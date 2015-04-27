using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float maxHealth;
	public float currentHealth;
	public HitCube hitCube;
	bool haveLandedAHit;

	void Start () {
		hitCube = transform.Find("HitCube").gameObject.GetComponent<HitCube>();
	}

	void Update(){
		if(haveLandedAHit == false && amFacingMonster() && amAttacking()){
			hitCube.monster.currentHealth -= 10;
			haveLandedAHit = true;
		}

		if(!amAttacking() && haveLandedAHit){
			haveLandedAHit = false;
		}
	}

	bool amAttacking(){
		return GetComponent<ThirdPersonControllerCS>().attacking;
	}

	bool amFacingMonster(){
		return hitCube.playerIsFaceMonster();
	}


}
