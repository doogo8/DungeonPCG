using UnityEngine;
using System.Collections;

public class GoblinAI : MonoBehaviour {
	public Collider[] hitColliders;
	public float aggroRange = 7f;
	public bool playerInAggroRange;

	void Start () {
	
	}

	void Update () {
		if(playerIsInAggroRange()){

		}else{
			roam();
		}


	}

	bool playerIsInAggroRange(){
		hitColliders = Physics.OverlapSphere(transform.position, aggroRange);

		for(int i = 0; i < hitColliders.Length; i++){
			if(hitColliders[i].gameObject.tag == "Player"){
				return true;
			}
		}
		return false;
	}

	void roam(){

	}
}
