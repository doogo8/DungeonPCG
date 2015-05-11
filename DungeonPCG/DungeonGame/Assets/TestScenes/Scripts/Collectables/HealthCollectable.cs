using UnityEngine;
using System.Collections;

namespace ProD
{
	public class HealthCollectable : MonoBehaviour {

		public float healingAmount = 25;	//amount of health that this object heals

		//This function is called when something collides with this object
		void OnTriggerEnter(Collider other){
			//if player collided with this object, heal the player
			if(other.tag == "Player") {
				Player player = other.gameObject.GetComponent<Player>();
				if(!player.healthIsFull()) {
					player.addHealth(healingAmount);
					Destroy(gameObject);
				}
			}
		}
	}
}