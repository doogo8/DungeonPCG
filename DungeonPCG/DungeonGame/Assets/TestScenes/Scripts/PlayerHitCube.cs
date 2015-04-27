using UnityEngine;
using System.Collections;

public class PlayerHitCube : MonoBehaviour {
	private bool facingEnemy;
	public Monster monster;
	
	void OnTriggerStay(Collider other){
		if(other.gameObject.tag == "Monster"){
			facingEnemy = true;
			monster = other.gameObject.GetComponent<Monster>();
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Monster"){
			facingEnemy = false;
		}
	}

	public bool playerIsFaceMonster(){
		return facingEnemy;
	}
}
