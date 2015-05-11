using UnityEngine;
using System.Collections;

namespace ProD 
{

	public class ChestCollectable : MonoBehaviour {
	
		Animation chestAnim;

		// Use this for initialization
		void Start () {
			chestAnim = gameObject.GetComponent<Animation>();
			chestAnim.Play("open");
			chestAnim.Sample();
			chestAnim.Stop();
		}
		
		// Update is called once per frame
		void Update () {

		}

		void OnTriggerEnter(Collider other) {
			if(other.tag == "Player") {
				chestAnim.Play("open");
				Destroy(this);
			}
		}
	}

}
