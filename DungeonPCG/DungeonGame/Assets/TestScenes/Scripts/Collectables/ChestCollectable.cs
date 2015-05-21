using UnityEngine;
using System.Collections;

namespace ProD 
{

	public class ChestCollectable : MonoBehaviour {
	
		Animation chestAnim;
		bool isActiveChest;
		bool isOpening;
		float openingTime = 0;
		private static ChestCollectable activeChest = null;

		// Use this for initialization
		void Start () {
			chestAnim = gameObject.GetComponent<Animation>();
			chestAnim.Play("open");
			chestAnim.Sample();
			chestAnim.Stop();
		}
		
		// Update is called once per frame
		void Update () {
			if(isActiveChest) {
				if(Input.GetKeyDown(KeyCode.E)) {
					activeChest = null;
					chestAnim.Play("open");
					isOpening = true;
					Destroy(this.GetComponent<Collider>());
					UseKeyUI.banishUseKeyUI();
				}
			}
			if(isOpening) {
				openingTime += Time.deltaTime;
				if(openingTime >= 1) {
					CoinCollectable.spawnCoins(transform.position + Vector3.up*0.25f, 5);
					Destroy(this);
				}
			}
		}


		// If this collides with player, set this chest to be the active chest (i.e. player
		//		can open the active chest by pressing the 'use' key)
		void OnTriggerEnter(Collider other) {
			if(other.tag == "Player") {
				if(activeChest != null)
					activeChest.isActiveChest = false;
				activeChest = this;
				isActiveChest = true;
				UseKeyUI.summonUseKeyUI(transform.position + Vector3.up*.5f);
			}
		}

		// When player leaves the active area of chest, deactivate this chest
		void OnTriggerExit(Collider other) {
			if(other.tag == "Player") {
				if(isActiveChest) {
					isActiveChest = false;
					activeChest = null;
					UseKeyUI.banishUseKeyUI();
				}
			}
		}

	}//end ChestCollectable class

}//end ProD namespace
