using UnityEngine;
using System.Collections;

//Singleton class for interacting with the "Use Key UI"
public class UseKeyUI : MonoBehaviour {

	private static GameObject useKeyUI;
	private static Vector3 banishLocation = new Vector3(0,0,3000000);

	// Use this for initialization
	void Start () {
		//Singleton functionality, destroy this if another instance exists
		if(useKeyUI != null)
			Destroy(this);
		useKeyUI = gameObject;
		UseKeyUI.banishUseKeyUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Use this to set location of the UseKeyUI
	public static void summonUseKeyUI(Vector3 location){
		useKeyUI.transform.position = location;
	}

	//Use this to make the UseKeyUI dissapear
	public static void banishUseKeyUI() {
		useKeyUI.transform.position = banishLocation;
	}
}
