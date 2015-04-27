using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour
{
	public class AttackType{
		public const string Melee = "Melee";
		public const string Ranged = "Ranged";
	}

	public class Damage{
		public const float Low = 5f;
		public const float Medium = 10f;
		public const float High = 20f;
	}

	public class Speed{
		public const float Low = 0.05f;
		public const float Medium = 0.1f;
		public const float High = 0.2f;
	}

	public class Health{
		public const float Low = 50f;
		public const float Medium = 100f;
		public const float High = 200f;
	}
	
	public class AggroRange{
		public const float Low = 5f;
		public const float Medium = 7f;
		public const float High = 10f;
	}

	public float currentHealth;
	public float maxHealth;

	public float aggroRange = 7f;
	public float chaseSpeed = 0.6f;
	public float roamSpeed = 0.06f;
	public string attackType;
	
	public GameObject player;
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	public List<AnimationClip> attackAnimations;
	public Animation _animation;

	private bool arrivedAtRoamingDestination;
	private Vector3 randomNearbyLocation;
	private Collider[] hitColliders;
	private bool aggroingPlayer;

	void Awake() {
		_animation = GetComponent<Animation> ();
		randomNearbyLocation = new Vector3 ();
	}

	void Update ()
	{
		if(aggroingPlayer && !reachedPlayer()){
			chase ();
		}else if (aggroingPlayer && reachedPlayer()){
			attack ();
		}else if(!aggroingPlayer){
			roam ();
		}

		if (spottedPlayer ()) {
			aggroingPlayer = true;
		}
	}

	bool spottedPlayer ()
	{
		hitColliders = Physics.OverlapSphere (transform.position, aggroRange);
		
		for (int i = 0; i < hitColliders.Length; i++) {
			if (hitColliders [i].gameObject.tag == "Player") {
				return true;
			}
		}
		return false;
	}

	void roam ()
	{
		_animation.CrossFade (walkAnimation.name);
		
		if (arrivedAtRoamingDestination == false) {
			randomNearbyLocation = 
				new Vector3 (transform.position.x + Random.Range (-5f, 5f), 
				             transform.position.y, 
				             transform.position.z + Random.Range (-5f, 5f));
			arrivedAtRoamingDestination = true;
		}
		
		lerpTowardTargetLocation (randomNearbyLocation, roamSpeed);
		
		if (transform.position == randomNearbyLocation)
			arrivedAtRoamingDestination = false;
		
	}

	void chase ()
	{
		lerpTowardTargetLocation (player.transform.position, chaseSpeed);
		_animation.CrossFade (runAnimation.name);
	}

	bool reachedPlayer ()
	{
		if (Mathf.Abs (player.transform.position.x - transform.position.x) < 2f
			&& Mathf.Abs (player.transform.position.z - transform.position.z) < 2f)
			return true;
		else
			return false;
	}

	void attack ()
	{
		bool attackClipPlaying = false;

		for(int i = 0; i < attackAnimations.Count; i++){
			if(_animation.IsPlaying(attackAnimations[i].name))
				attackClipPlaying = true;
		}

		if(!attackClipPlaying){
			_animation.CrossFade (attackAnimations[(int)Mathf.Floor(Random.Range(0f, (float)attackAnimations.Count))].name);
		}
	}
	
	void lerpTowardTargetLocation (Vector3 targetPos, float speed)
	{
		transform.forward = Vector3.Lerp (transform.forward, (targetPos - transform.position), Time.deltaTime * 3f);
		transform.position = Vector3.MoveTowards (transform.position, targetPos, speed);
	}
}
