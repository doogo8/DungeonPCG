using UnityEngine;
using System.Collections;

public class GoblinAI : MonoBehaviour
{
	public Collider[] hitColliders;
	public GameObject player;
	public float aggroRange = 7f;
	public float chaseSpeed = 0.1f;
	public float roamSpeed = 0.05f;
	public bool playerInAggroRange;
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	private Animation _animation;
	public bool arrivedAtRoamingDestination;
	Vector3 randomNearbyLocation;
	
	void Awake ()
	{
		_animation = GetComponent<Animation> ();
		randomNearbyLocation = new Vector3();
	}

	void Update ()
	{
		if (playerIsInAggroRange ()) {
			attack ();
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

		if(transform.position == randomNearbyLocation)
			arrivedAtRoamingDestination = false;

	}

	void attack ()
	{
		lerpTowardTargetLocation (player.transform.position, chaseSpeed);
		_animation.CrossFade (runAnimation.name);
	}

	void lerpTowardTargetLocation (Vector3 targetPos, float speed)
	{
		transform.forward = Vector3.Lerp (transform.forward, (targetPos - transform.position), Time.deltaTime * 3f);
		transform.position = Vector3.MoveTowards (transform.position, targetPos, speed);
	}
}
