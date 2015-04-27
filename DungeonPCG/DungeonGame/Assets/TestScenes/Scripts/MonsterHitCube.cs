using UnityEngine;
using System.Collections;

public class MonsterHitCube : MonoBehaviour {
	private bool facingPlayer;
	public Player player;
	
	void OnTriggerStay(Collider other){
		if(other.gameObject.tag == "Player"){
			facingPlayer = true;
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			facingPlayer = false;
		}
	}
	
	public bool monsterIsFacePlayer(){
		return facingPlayer;
	}
}
