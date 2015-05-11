using UnityEngine;
using System.Collections;

namespace ProD 
{

	public class CoinCollectable : MonoBehaviour {
	
		public int coinAmount = 1;	//amount of coins gained from this chest

		//This function is triggered when other collider enters this object's collider
		void OnTriggerEnter(Collider other) {
			//Check if other collider belongs to player
			if(other.tag == "Player") {
				other.gameObject.GetComponent<Player>().collectCoins(coinAmount);
				Destroy(this.gameObject);
			}
		}
	}

}
