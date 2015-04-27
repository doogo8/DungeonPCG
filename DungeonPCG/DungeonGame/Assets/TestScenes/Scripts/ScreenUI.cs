using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour {
	public Canvas screenUI;
	public Slider healthBar;
	public Player player;

	void Start () {
		screenUI = this.gameObject.GetComponent<Canvas>();
		healthBar = transform.Find("HealthBar").gameObject.GetComponent("Slider") as Slider;
	}
	
	void Update () {
		healthBar.value = (player.currentHealth / player.maxHealth);
	}
}
