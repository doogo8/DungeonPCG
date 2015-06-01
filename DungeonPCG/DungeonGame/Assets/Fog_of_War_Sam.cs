using UnityEngine;
using System.Collections;

public class Fog_of_War_Sam : MonoBehaviour {

	//If the player gets close to a fog tile, that area
	//is explored and the tile will disappear now.
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Player")) {
			Destroy(this.gameObject);
		}
	}


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
