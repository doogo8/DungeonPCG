using UnityEngine;
using System.Collections;

public class CollectableBounce : MonoBehaviour {


	private float masterScalar = .7f;
	private float sideMult = 0.007f;
	private Vector2 sideVec;
	private float initialVelocity = .07f;
	private float bounceVelocity;
	private float bounceAccel = -0.003f;
	int bounceCount = 0;
	bool collidedWall = false;



	// Use this for initialization
	public void init () {
		sideMult *= masterScalar;
		initialVelocity *= masterScalar;
		bounceAccel *= masterScalar;
		float randAngle = Random.Range(0, Mathf.PI*2);
		sideVec.x = Mathf.Cos(randAngle);
		sideVec.y = Mathf.Sin(randAngle);
		bounceVelocity = initialVelocity;
	}
	
	// Update is called once per frame
	void Update () {
		if(!collidedWall)
			transform.Translate(new Vector3(sideVec.x * sideMult, 0, sideVec.y * sideMult), Space.World);
		bounceVelocity += bounceAccel;
		transform.Translate(Vector3.up * bounceVelocity, Space.World);
	}

	//When colliding with ground, bounce. When colliding with wall, stop moving to sides
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Wall")
			collidedWall = true;
		if(other.tag == "Floor"){
			bounceCount++;
			if(bounceCount >= 2) {
				transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
				Destroy(this);
			}
			initialVelocity *= 0.6f;
			bounceVelocity = initialVelocity;
			//transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
		}
	}
}
