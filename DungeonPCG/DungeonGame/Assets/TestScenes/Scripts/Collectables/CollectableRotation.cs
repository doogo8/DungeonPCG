using UnityEngine;
using System.Collections;

public class CollectableRotation : MonoBehaviour {

	public float rotationSpeed = 200;

	// Use this for initialization
	void Start () {
		transform.Rotate(Vector3.up * Random.Range(0.0f, 360.0f));
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
	}
}
