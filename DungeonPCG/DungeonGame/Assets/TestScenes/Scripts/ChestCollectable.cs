using UnityEngine;
using System.Collections;

namespace ProD 
{

	public class ChestCollectable : MonoBehaviour {
	
		public int coinAmount = 5;	//amount of coins gained from this chest
		Animation chestAnim;

		// Use this for initialization
		void Start () {
			chestAnim = gameObject.GetComponent<Animation>();
			chestAnim.Play("open");
			chestAnim.Stop();
		//	chestAnim.
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnTriggerEnter(Collider other) {
			if(other.tag == "Player") {
				other.gameObject.GetComponent<Player>().collectCoins(coinAmount);
				chestAnim.Play("open");
				Destroy(this);
			}
		}
	}

}
