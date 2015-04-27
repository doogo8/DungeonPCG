using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour
{
	private Collider[] hitColliders;
	public GameObject player;
	public float aggroRange = 7f;
	public float chaseSpeed = 0.6f;
	public float roamSpeed = 0.06f;
	private bool playerInAggroRange;
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	public List<AnimationClip> attackAnimations;
	public Animation _animation;
	private bool arrivedAtRoamingDestination;
	private Vector3 randomNearbyLocation;

	void Start ()
	{
		_animation = GetComponent<Animation> ();
		randomNearbyLocation = new Vector3 ();
	}

	void Update ()
	{
		if (playerIsInAggroRange ()) {
			if(!reachedPlayer()){
				chase ();
			}else{
				attack ();
			}
		} else {
			roam ();
		}
	}

	bool playerIsInAggroRange ()
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
