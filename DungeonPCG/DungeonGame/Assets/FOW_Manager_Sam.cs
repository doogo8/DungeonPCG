using UnityEngine;
using System.Collections;

public class FOW_Manager_Sam : MonoBehaviour {

	public GameObject tile;
	public float zSpawn;
	public float xRange = 10f;
	public float yRange = 10f;

	private float myX;
	private float myY;
	private float tileSize = 4;

	// Use this for initialization
	void Start () {
		myX = GetComponent<Transform> ().position.x;
		myY = GetComponent<Transform> ().position.y;
		//tileSize = tile.GetComponent<Collider> ().bounds.size.x;

		for (float i = myX - xRange; i <= myX +xRange; i += tileSize) {
			for (float j = myY - yRange; i <= myY +yRange; i += tileSize) {
				Instantiate (tile, new Vector3(i , j , zSpawn), Quaternion.identity);
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (tileSize);
	}
}
