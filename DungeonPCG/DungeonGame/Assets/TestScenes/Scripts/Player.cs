using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace ProD
{
	public class Player : MonoBehaviour
	{
		public AnimationClip dieAnimation;
		public Animation _animation;

		public float maxHealth;
		public float currentHealth;
		private int currentCoins;

		// UI
		private Text coinUI;

		// collision
		public GameObject hitCube;

		public List<Monster> hitList;
		bool haveLandedAHit;

		// death
		public bool dead;
		bool deathClipDone;
		bool deathClipStarted;

		// audio
		public AudioClip swooshSound;
		public AudioClip hitSound;
		private AudioSource audioSource;
		private float lowPitchRange = .75F;
		private float highPitchRange = 1.5F;

		//The following will be retrieved by DataLogger
		public int totalDamageToEnemies;
		public int totalAttacks;
	
		void Start ()
		{	
			audioSource = GetComponent<AudioSource> ();
			hitCube = transform.Find ("PlayerHitCube").gameObject;
			//hitCube.transform.localScale = new Vector3 (.04f, .04f, .04f);
			_animation = GetComponent<Animation> ();

			maxHealth = 100f;
			currentHealth = 100f;

			deathClipDone = false;

			GameObject.Find ("ScreenUI").SetActive (true);

			//Try and access the coin UI object, if it doesn't exist print an error
			coinUI = GameObject.Find("CoinCount").GetComponent<Text>();
			if(coinUI == null)
				Debug.Log ("No coin UI object found in scene, must be named 'CoinCount' and have a 'Text' Component attached");
			//Call collectCoins with value of 0 to initialize the coins ui
			collectCoins(0);
		}
	
		void Update ()
		{

			checkForDeath ();
		
			if (!dead) {
				collectMonstersImFacing ();
			
				if (haveLandedAHit == false && amAttacking () && hitList.Count > 0) {
					foreach (Monster m in hitList) {
						m.currentHealth -= 10;
						totalDamageToEnemies += 10;
					}
					audioSource.pitch = Random.Range (lowPitchRange, highPitchRange);
					audioSource.PlayOneShot (hitSound, 1f);
					haveLandedAHit = true;
				}
			
				if (!amAttacking () && haveLandedAHit) {
					haveLandedAHit = false;
					audioSource.pitch = Random.Range (lowPitchRange, highPitchRange);
					audioSource.PlayOneShot (swooshSound, 1f);
				}
			} else {
				die ();
			}
		}
	
		void checkForDeath ()
		{
			if (currentHealth <= 0f) 
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
	
		void die ()
		{
			// destroy components
			if (GetComponent<ThirdPersonControllerCS> () != null)
				Destroy (GetComponent<ThirdPersonControllerCS> ());
		
			if (deathClipStarted == false) {
				_animation.CrossFade (dieAnimation.name);
				StartCoroutine (WaitForAnimationToEnd (dieAnimation));
				deathClipStarted = true;
			}
		
			if (deathClipDone) {
				_animation.enabled = false;
			}

			// update stats
			DataLogger dl = GameObject.Find ("DataLogger").GetComponent ("DataLogger") as DataLogger;
			dl.totalDamageToEnemies += totalDamageToEnemies;
			dl.totalPlayerAttacks += totalAttacks;
//			dl.
		}
	
		bool amAttacking ()
		{
			return GetComponent<ThirdPersonControllerCS> ().attacking;
		}
	
		void collectMonstersImFacing ()
		{
			Collider[] hitColliders = Physics.OverlapSphere (hitCube.transform.position, .25f); //reduced attack range of player -Bryan
			hitList.Clear ();
		
			int i = 0;
			while (i < hitColliders.Length) {
				if (hitColliders [i].gameObject.GetComponent<Monster> () != null) {
					hitList.Add (hitColliders [i].gameObject.GetComponent<Monster> ());
				}
				i++;
			}
		}
	
		//Updates the value stored in 'currentCoins' and also updates the UI
		public void collectCoins(int numCoins) {
			currentCoins += numCoins;
			coinUI.text = currentCoins.ToString();
		}

		//Checks if health is full
		public bool healthIsFull() {
			return currentHealth == maxHealth;
		}

		//Adds an amount of health to the player
		public void addHealth(float amount) {
			currentHealth += amount;
			if(currentHealth > maxHealth)
				currentHealth = maxHealth;
		}
	}
}
