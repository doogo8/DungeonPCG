using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
	public Canvas monsterUI;
	public Slider healthBar;
	
	void Start ()
	{
		monsterUI = this.gameObject.GetComponent<Canvas> ();
		healthBar = transform.Find ("HealthBar").gameObject.GetComponent ("Slider") as Slider;
	}

	void Update ()
	{
		if (transform.parent.gameObject.GetComponent<Monster> () != null) {
			healthBar.value = (transform.parent.gameObject.GetComponent<Monster> ().currentHealth / 
				transform.parent.gameObject.GetComponent<Monster> ().maxHealth);
		}
	}

	void LateUpdate ()
	{
		monsterUI.transform.rotation = Quaternion.Euler (Camera.main.transform.rotation.eulerAngles);
	}
}
