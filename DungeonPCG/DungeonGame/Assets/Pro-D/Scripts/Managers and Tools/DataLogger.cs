using UnityEngine;
using System.Collections;

namespace ProD
{
		public class DataLogger : MonoBehaviour
		{

				public GameObject player;
				public GameObject exampleSceneGUI;
				//The following will be fetched from other scripts
				public int numJumps;
				public int totalDamageToEnemies;
				public float totalEnemies;
				public float enemiesKilled;
				public float enemiesKilledPercentage;

				//This is incremented in Monster.cs
				public float totalDamageToPlayer;

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

						//GET EXAMPLESCENE GUI
						if (GameObject.Find ("ExampleSceneGUI") != null) 
							exampleSceneGUI = GameObject.Find ("ExampleSceneGUI");


						//Get jumps and total damage to enemies
						if (player != null) {
								ThirdPersonControllerCS tpc = player.GetComponent<ThirdPersonControllerCS> ();
								if (tpc != null) {
										numJumps = tpc.getNumJumps ();
								}
				
								//Get total damage to enemies from Player.cs script
								totalDamageToEnemies = player.GetComponent<Player> ().totalDamageToEnemies;
						}
						//Get total number of enemies in the level
						if (exampleSceneGUI != null) {
							totalEnemies = exampleSceneGUI.GetComponent<ExampleSceneGUI>().getEnemyFrequency();
						}
						enemiesKilledPercentage = enemiesKilled / totalEnemies;

				}

				void Exit ()
				{

				}
		}
}