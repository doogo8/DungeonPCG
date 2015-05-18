using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProD{
	public class Monster : MonoBehaviour
	{
		public enum AttackType{
			Melee = 1,
			Ranged = 2
		}

		public class Damage{
			public const float Low = 2f;
			public const float Medium = 10f;
			public const float High = 20f;
		}

		public class Speed{
			public const float Low = .5f;
			public const float Medium = 1f;
			public const float High = 2f;
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

		public GameObject hitCube;

		public float currentHealth;
		public float maxHealth;
		public bool dead;
		bool deathClipDone;
		bool deathClipStarted;
		public bool ableToBeHit;

		public float aggroRange = 7f;
		public float chaseSpeed = 0.6f;
		public float roamSpeed = 0.06f;
		public float attackDamage = 10f;
		public AttackType attackType;
		
		public GameObject player;
		public AnimationClip idleAnimation;
		public AnimationClip walkAnimation;
		public AnimationClip runAnimation;
		public AnimationClip dieAnimation;
		public List<AnimationClip> attackAnimations;
		public Animation _animation;

		private bool arrivedAtRoamingDestination;
		private Vector3 randomNearbyLocation;
		private Collider[] hitColliders;
		private bool aggroingPlayer;
		private bool haveLandedAHit;

		//For DataLogger
		public GameObject dataLogger;

		void Awake() {
			_animation = GetComponent<Animation> ();
			randomNearbyLocation = new Vector3 ();
			hitCube = transform.Find("MonsterHitCube").gameObject;
			//hitCube.transform.localScale = new Vector3 (.04f, .04f, .04f);

			//Get DataLogger
			if (GameObject.Find ("DataLogger") != null)
				dataLogger = GameObject.Find ("DataLogger");
		}

		void Update ()
		{
			checkForDeath();

			if(!dead){
				if(player.GetComponent<Player>() != null){ //would mean player is dead
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
				}else{
					roam ();
				}
			}else{
				die();
			}

		}

		void checkForDeath(){
			if (currentHealth <= 0f) {
			dead = true;
			}
			else
				dead = false;
		}
		
		void die(){
			if(deathClipStarted == false){
				_animation.CrossFade(dieAnimation.name);
				deathClipStarted = true;
			}

			if(!_animation.IsPlaying(dieAnimation.name)){
				deathClipDone = true;
			}

			if(deathClipDone){
				dataLogger.GetComponent<DataLogger> ().enemiesKilled += 1;
				Component[] g = GetComponents(typeof(Component));

				foreach(Component comp in g){
					if(!(comp is UnityEngine.Transform) && !(comp is Monster))
						Destroy (comp);
				}

				Destroy (GetComponent<Monster>());
				Destroy (transform.Find("MonsterUI").gameObject);
				Destroy (transform.Find("MonsterHitCube").gameObject);
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
			if (Mathf.Abs (player.transform.position.x - transform.position.x) < .4f
				&& Mathf.Abs (player.transform.position.z - transform.position.z) < .4f) //reduced the distance necessary for enemy to start attacking - Bryan
				return true;
			else
				return false;
		}

		bool amIFacingPlayer(){
			Collider[] hitColliders = Physics.OverlapSphere(hitCube.transform.position, 2f);
			
			int i = 0;
			while (i < hitColliders.Length) {
				if(hitColliders[i].gameObject.GetComponent<Player>() != null){
					return true;
				}
				i++;
			}

			return false;
		}

		void attack ()
		{
			bool attackClipPlaying = false;

			for(int i = 0; i < attackAnimations.Count; i++){
				if(_animation.IsPlaying(attackAnimations[i].name))
					attackClipPlaying = true;
			}

			if(haveLandedAHit == false && amIFacingPlayer() && attackClipPlaying){
				player.GetComponent<Player>().currentHealth -= attackDamage;
				dataLogger.GetComponent<DataLogger>().totalDamageToPlayer += attackDamage;
				haveLandedAHit = true;
			}

			transform.forward = Vector3.Lerp (transform.forward, (player.transform.position - transform.position), Time.deltaTime * 3f);
			
			if(!attackClipPlaying && haveLandedAHit){
				haveLandedAHit = false;
			}

			if(!attackClipPlaying){
				_animation.CrossFade (attackAnimations[(int)Mathf.Floor(Random.Range(0f, (float)attackAnimations.Count))].name);
			}
		}
		
		void lerpTowardTargetLocation (Vector3 targetPos, float speed)
		{
			transform.forward = Vector3.Lerp (transform.forward, (targetPos - transform.position), Time.deltaTime * 3f);

			Vector3 moveDiff = targetPos - transform.position;
			Vector3 movDir = moveDiff.normalized * speed * Time.deltaTime;

			if(movDir.sqrMagnitude < moveDiff.sqrMagnitude)
			{
				GetComponent<CharacterController>().Move(movDir);
			}
			else
			{
				GetComponent<CharacterController>().Move(moveDiff);
			}
		}
	}
}
