using UnityEngine;
using System.Collections;

namespace ProD
{
		public class DataLogger : MonoBehaviour
		{

				public GameObject player;
				public int numJumps;
				public int totalDamageToEnemies;

				// Use this for initialization
				void Start ()
				{
	
				}
	
				// Update is called once per frame
				void Update ()
				{

						//GET PLAYER
						if (GameObject.Find ("adventurer(Clone)") != null) {
								player = GameObject.Find ("adventurer(Clone)");
						}
						if (GameObject.Find ("ai-test-adventurer(Clone)") != null) {
								player = GameObject.Find ("ai-test-adventurer(Clone)");
						}
						if (GameObject.Find ("ai-test-adventurer w_UI(Clone)") != null) {
								player = GameObject.Find ("ai-test-adventurer w_UI(Clone)");
						}
						//


						//Get jumps
						numJumps = player.GetComponent<ThirdPersonControllerCS> ().numJumps;
						//Get total damage to enemies
						totalDamageToEnemies = player.GetComponent<Player>().totalDamageToEnemies;
				}

				void Exit ()
				{

				}
		}
}