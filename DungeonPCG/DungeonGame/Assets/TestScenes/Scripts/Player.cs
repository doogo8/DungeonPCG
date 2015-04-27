using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public float maxHealth;
	public float currentHealth;
	public GameObject hitCube;
	public List<Monster> hitList;
	bool haveLandedAHit;

	void Start () {
		hitCube = transform.Find("PlayerHitCube").gameObject;
		maxHealth = 100f;
		currentHealth = 100f;
	}

	void Update(){

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
