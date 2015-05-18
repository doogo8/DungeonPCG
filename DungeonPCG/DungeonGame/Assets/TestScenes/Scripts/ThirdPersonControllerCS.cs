﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Rayco's scripts/third person controller")]
public class ThirdPersonControllerCS : MonoBehaviour {
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	public AnimationClip jumpPoseAnimation;
	public List<AnimationClip> attackAnimations;
	int attackClipIndex;
	public AnimationClip sword1Animation;
	public AnimationClip sword2Animation;
	public AnimationClip sword3Animation;
	
	public float walkMaxAnimationSpeed = 0.75F;
	public float trotMaxAnimationSpeed = 1F;
	public float runMaxAnimationSpeed = 1F;
	public float jumpAnimationSpeed = 1F;
	public float landAnimationSpeed =1F;

	public bool attacking;
	
	private Animation _animation;

	public int numJumps = 0;
	
	enum CharacterState {
		Idle = 0,
		Walking = 1,
		Trotting = 2,
		Running = 3,
		Jumping = 4,   
		Sword1 = 5,
		Sword2 = 6,
		Sword3 = 7,
	}
	private CharacterState _characterState;
	// The speed when walking
	public float walkSpeed = 2.0F;
	// after trotAfterSeconds of walking we trot with trotSpeed
	public float trotSpeed = 4.0F;
	// when pressing "Fire3" button (cmd) we start running
	public float runSpeed = 6.0F;
	
	public float inAirControlAcceleration = 3.0F;
	
	// How high do we jump when pressing jump and letting go immediately
	public float jumpHeight = 0.5F;
	
	// The gravity for the character
	public float gravity = 20.0F;
	// The gravity in controlled descent mode
	public float speedSmoothing = 10.0F;
	public float rotateSpeed = 500.0F;
	public float trotAfterSeconds = 3.0F;
	
	public bool canJump= true;
	
	private float jumpRepeatTime = 0.05F;
	private float jumpTimeout = 0.15F;
	private float groundedTimeout = 0.25F;
	
	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer = 0.0F;
	
	// The current move direction in x-z
	private Vector3 moveDirection = Vector3.zero;
	// The current vertical speed
	private float verticalSpeed = 0.0F;
	// The current x-z move speed
	private float moveSpeed = 0.0F;
	
	// The last collision flags returned from controller.Move
	private CollisionFlags collisionFlags ;
	
	// Are we jumping? (Initiated with jump button and not grounded yet)
	private bool jumping = false;
	private bool jumpingReachedApex = false;
	
	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack = false;
	// Is the user pressing any keys?
	private bool isMoving = false;
	// When did the user start walking (Used for going into trot after a while)
	private float walkTimeStart = 0.0F;
	// Last time the jump button was clicked down
	private float lastJumpButtonTime = -10.0F;
	// Last time we performed a jump
	private float lastJumpTime = -1.0F;
	
	
	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
	private float lastJumpStartHeight = 0.0F;
	
	
	private Vector3 inAirVelocity = Vector3.zero;
	
	private float lastGroundedTime = 0.0F;
	
	
	private bool isControllable = true;

	//Audio stuff
	public AudioClip footstepSound;
	private AudioSource audioSource;
	private float lowPitchRange = .75F;
	private float highPitchRange = 1.5F;
	
	// Use this for initialization
	void  Awake (){
		audioSource = GetComponent<AudioSource>();
		moveDirection = transform.TransformDirection(Vector3.forward);

		attackAnimations.Add(sword1Animation);
		attackAnimations.Add(sword2Animation);
		attackAnimations.Add(sword3Animation);
		
		_animation = GetComponent<Animation>();
		if(!_animation)
			Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
		
		/*
public AnimationClip idleAnimation;
public AnimationClip walkAnimation;
public AnimationClip runAnimation;
public AnimationClip jumpPoseAnimation;
    */
		if(!idleAnimation) {
			_animation = null;
			Debug.Log("No idle animation found. Turning off animations.");
		}
		if(!walkAnimation) {
			_animation = null;
			Debug.Log("No walk animation found. Turning off animations.");
		}
		if(!runAnimation) {
			_animation = null;
			Debug.Log("No run animation found. Turning off animations.");
		}
		if(!jumpPoseAnimation && canJump) {
			_animation = null;
			Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
		}
		if(!sword1Animation || !sword2Animation || !sword3Animation) {
			_animation = null;
			Debug.Log("No attack animations found. Turning off animations.");
		}
		
	}
	void  UpdateSmoothedMovementDirection (){
		Transform cameraTransform = Camera.main.transform;
		bool grounded = IsGrounded();
		
		// Forward vector relative to the camera along the x-z plane   
		Vector3 forward= cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right= new Vector3(forward.z, 0, -forward.x);
		
		float v= Input.GetAxisRaw("Vertical");
		float h= Input.GetAxisRaw("Horizontal");
		
		// Are we moving backwards or looking backwards
		if (v < -0.2f)
			movingBack = true;
		else
			movingBack = false;
		
		bool wasMoving= isMoving;
		isMoving = Mathf.Abs (h) > 0.1f || Mathf.Abs (v) > 0.1f;
		
		// Target direction relative to the camera
		Vector3 targetDirection= h * right + v * forward;
		
		// Grounded controls
		if (grounded)
		{
			// Lock camera for short period when transitioning moving  standing still
			lockCameraTimer += Time.deltaTime;
			if (isMoving != wasMoving)
				lockCameraTimer = 0.0f;
			
			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update it if there is user input.
			if (targetDirection != Vector3.zero)
			{
				// If we are really slow, just snap to the target direction
				if (moveSpeed < walkSpeed * 0.9f && grounded)
				{
					moveDirection = targetDirection.normalized;
				}
				// Otherwise smoothly turn towards it
				else
				{
					moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					
					moveDirection = moveDirection.normalized;
				}
			}
			
			// Smooth the speed based on the current target direction
			float curSmooth= speedSmoothing * Time.deltaTime;
			
			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			float targetSpeed= Mathf.Min(targetDirection.magnitude, 1.0f);
			
			_characterState = CharacterState.Idle;
			
			// Pick speed modifier
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
			{
				targetSpeed *= runSpeed;
				_characterState = CharacterState.Running;
			}
			else if (Time.time - trotAfterSeconds > walkTimeStart)
			{
				targetSpeed *= trotSpeed;
				_characterState = CharacterState.Trotting;
			}
			else
			{
				targetSpeed *= walkSpeed;
				_characterState = CharacterState.Walking;
			}
			
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
			
			// Reset walk time start when we slow down
			if (moveSpeed < walkSpeed * 0.3f)
				walkTimeStart = Time.time;
		}
		// In air controls
		else
		{
			// Lock camera while in air
			if (jumping)
				lockCameraTimer = 0.0f;
			
			if (isMoving)
				inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
		}
		
		
		
	}
	void  ApplyJumping (){
		// Prevent jumping too fast after each other
		if (lastJumpTime + jumpRepeatTime > Time.time)
			return;
		
		if (IsGrounded()) {
			// Jump
			// - Only when pressing the button down
			// - With a timeout so you can press the button slightly before landing
			if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	void  ApplyGravity (){
		if (isControllable) // don't move player at all if not controllable.
		{
			// Apply gravity
			bool jumpButton= Input.GetButton("Jump");
			
			
			// When we reach the apex of the jump we send out a message
			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
			{
				jumpingReachedApex = true;
				SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
			
			if (IsGrounded ())
				verticalSpeed = 0.0f;
			else
				verticalSpeed -= gravity * Time.deltaTime;
		}
	}
	float  CalculateJumpVerticalSpeed ( float targetJumpHeight  ){
		// From the jump height and gravity we deduce the upwards speed
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
	}  
	public void  DidJump (){
		jumping = true;
		jumpingReachedApex = false;
		lastJumpTime = Time.time;
		lastJumpStartHeight = transform.position.y;
		lastJumpButtonTime = -10;
		numJumps++;
		
		_characterState = CharacterState.Jumping;
	}

	public int getNumJumps() {
		return numJumps;
	}

	void  Update (){
		
		if (!isControllable)
		{
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
		}
		
		if (Input.GetButtonDown ("Jump"))
		{
			lastJumpButtonTime = Time.time;
		}
		
		UpdateSmoothedMovementDirection();
		
		// Apply gravity
		// - extra power jump modifies gravity
		// - controlledDescent mode modifies gravity
		ApplyGravity ();
		
		// Apply jumping logic
		ApplyJumping ();
		
		// Calculate actual motion
		Vector3 movement= moveDirection * moveSpeed + new Vector3 (0, verticalSpeed, 0) + inAirVelocity;
		movement *= Time.deltaTime;
		
		// Move the controller
		CharacterController controller = GetComponent<CharacterController>();
		collisionFlags = controller.Move(movement);

		// ANIMATION sector
		if(_animation) {
			if(_characterState == CharacterState.Jumping)
			{
				if(!jumpingReachedApex) {
					_animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
					_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					_animation.CrossFade(jumpPoseAnimation.name);
				} else {
					_animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
					_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					_animation.CrossFade(jumpPoseAnimation.name);              
				}
			}
			else
			{
				if(controller.velocity.sqrMagnitude < 0.1f) {
					_animation.CrossFade(idleAnimation.name);
				}
				else
				{
					if(_characterState == CharacterState.Running) {
						_animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
						_animation.CrossFade(runAnimation.name);   
						audioSource.loop = true;
						audioSource.pitch = Random.Range (lowPitchRange, highPitchRange);
						audioSource.PlayDelayed(1f);
					}
					else if(_characterState == CharacterState.Trotting) {
						_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, trotMaxAnimationSpeed);
						_animation.CrossFade(walkAnimation.name);  
						audioSource.loop = true;
						audioSource.pitch = Random.Range (lowPitchRange, highPitchRange);
						audioSource.PlayDelayed(1f);
					}
					else if(_characterState == CharacterState.Walking) {
						_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
						_animation.CrossFade(walkAnimation.name);
						audioSource.loop = true;
						if (!audioSource.isPlaying) {
							audioSource.pitch = Random.Range (lowPitchRange, highPitchRange);
							audioSource.Play();
						}
						else audioSource.Stop();
					}
					
				}
			}


			if(Input.GetMouseButtonDown(0)){
				if(!attacking){
					attackClipIndex = (int)Mathf.Floor(Random.Range(0f, (float)attackAnimations.Count));
					StartCoroutine(WaitForAnimationToEnd(attackAnimations[attackClipIndex]));
				}
			}

			if(attacking){
				_animation.CrossFade(attackAnimations[attackClipIndex].name);
			}
			
		}
		// ANIMATION sector
		
		// Set rotation to the move direction
		if (IsGrounded())
		{
			transform.rotation = Quaternion.LookRotation(moveDirection);
		}  
		else
		{
			Vector3 xzMove= movement;
			xzMove.y = 0;
			if (xzMove.sqrMagnitude > 0.001f)
			{
				transform.rotation = Quaternion.LookRotation(xzMove);
			}
		}  
		
		// We are in jump mode but just became grounded
		if (IsGrounded())
		{
			lastGroundedTime = Time.time;
			inAirVelocity = Vector3.zero;
			if (jumping)
			{
				jumping = false;
				SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private IEnumerator WaitForAnimationToEnd (AnimationClip a)
	{
		attacking = true;
		yield return new WaitForSeconds (a.length);
		attacking = false;
	}

	void  OnControllerColliderHit ( ControllerColliderHit hit   ){
		//  Debug.DrawRay(hit.point, hit.normal);
		if (hit.moveDirection.y > 0.01f)
			return;
	}  
	float  GetSpeed (){
		return moveSpeed;
	}
	
	bool  IsJumping (){
		return jumping;
	}
	
	bool  IsGrounded (){
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
	
	Vector3  GetDirection (){
		return moveDirection;
	}
	
	bool  IsMovingBackwards (){
		return movingBack;
	}
	
	float  GetLockCameraTimer (){
		return lockCameraTimer;
	}
	
	bool IsMoving (){
		return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
	}
	
	bool  HasJumpReachedApex (){
		return jumpingReachedApex;
	}
	
	bool  IsGroundedWithTimeout (){
		return lastGroundedTime + groundedTimeout > Time.time;
	}
	
	void  Reset (){
		gameObject.tag = "Player";
	}
	
}

