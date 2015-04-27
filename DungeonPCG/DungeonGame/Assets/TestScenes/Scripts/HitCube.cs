using UnityEngine;
using System.Collections;

public class HitCube : MonoBehaviour {
	private bool facingMonster;
	public Monster monster;
	
	void OnTriggerStay(Collider other){
		if(other.gameObject.tag == "Monster"){
			facingMonster = true;
			monster = other.gameObject.GetComponent<Monster>();
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Monster"){
			facingMonster = false;
			monster = null;
		}
	}

	public bool playerIsFaceMonster(){
		return facingMonster;
	}
}
