using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProD{
	public class Player : MonoBehaviour {
		public AnimationClip dieAnimation;
		public Animation _animation;

		public float maxHealth;
		public float currentHealth;
		public GameObject hitCube;
		public List<Monster> hitList;
		bool haveLandedAHit;
		public bool dead;
		bool deathClipDone;
		bool deathClipStarted;

		void Start () {
			hitCube = transform.Find("PlayerHitCube").gameObject;
			_animation = GetComponent<Animation> ();
			maxHealth = 100f;
			currentHealth = 100f;
			deathClipDone = false;
		}

		void Update()
		{


			checkForDeath();


			if(!dead){
				collectMonstersImFacing();
				
				if(haveLandedAHit == false && amAttacking() && hitList.Count > 0){
					foreach(Monster m in hitList){
						m.currentHealth -= 10;
					}
					haveLandedAHit = true;
				}

				if(!amAttacking() && haveLandedAHit){
					haveLandedAHit = false;
				}
			}else{
				die();
			}
		}

		void checkForDeath(){
			if(currentHealth <= 0f) 
				dead = true;
			else
				dead = false;
		}

		private IEnumerator WaitForAnimationToEnd (AnimationClip a)
		{
			deathClipDone = false;
			yield return new WaitForSeconds (a.length - 0.1f);
			deathClipDone = true;

		}
		
		void die(){
			if(GetComponent<ThirdPersonControllerCS>() != null)
				Destroy (GetComponent<ThirdPersonControllerCS>());

			if(deathClipStarted == false){
				_animation.CrossFade(dieAnimation.name);
				StartCoroutine(WaitForAnimationToEnd(dieAnimation));
				deathClipStarted = true;
			}

			if(deathClipDone){
				_animation.enabled = false;
				Component[] g = GetComponents(typeof(Component));
				
				foreach(Component comp in g){
					if(!(comp is UnityEngine.Transform) && !(comp is Player))
						Destroy (comp);
				}

				Destroy (transform.Find("PlayerHitCube").gameObject);
				Destroy (GetComponent<Player>());
			}
		}
		
		bool amAttacking(){
			return GetComponent<ThirdPersonControllerCS>().attacking;
		}

		void collectMonstersImFacing(){

			Collider[] hitColliders = Physics.OverlapSphere(hitCube.transform.position, 2f);
			hitList.Clear ();
			
			int i = 0;
			while (i < hitColliders.Length) {
				if(hitColliders[i].gameObject.GetComponent<Monster>() != null){
					hitList.Add(hitColliders[i].gameObject.GetComponent<Monster>());
				}
				i++;
			}
		}

	}
}
