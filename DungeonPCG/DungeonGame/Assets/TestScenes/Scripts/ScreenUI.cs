using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProD{
	public class ScreenUI : MonoBehaviour {
		public Canvas screenUI;
		public Slider healthBar;
		public Player player;

		void Start () {
			screenUI = this.gameObject.GetComponent<Canvas>();
			healthBar = transform.Find("HealthBar").gameObject.GetComponent("Slider") as Slider;
		}
		
		void Update () {
			if (player == null) {
				player = GameObject.Find ("ai-test-adventurer(Clone)").GetComponent("Player") as Player;
			}
			else healthBar.value = (player.currentHealth / player.maxHealth);
		}
	}
}
