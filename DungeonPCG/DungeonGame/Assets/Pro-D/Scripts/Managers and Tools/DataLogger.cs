using UnityEngine;
using System.Collections;

public class DataLogger : MonoBehaviour {

	public GameObject player;
	public int numJumps;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Find the player so we can get information about the player.
		if(GameObject.Find("adventurer(Clone)") != null){
			player = GameObject.Find("adventurer(Clone)");
		}
		if(GameObject.Find("ai-test-adventurer(Clone)") != null){
			player = GameObject.Find("ai-test-adventurer(Clone)");
		}
		if(GameObject.Find("ai-test-adventurer w_UI(Clone)") != null){
			player = GameObject.Find("ai-test-adventurer w_UI(Clone)");
		}

		//Get jumps
		numJumps = player.GetComponent<ThirdPersonControllerCS>().numJumps;
	
	}

	void Exit() {

	}
}
