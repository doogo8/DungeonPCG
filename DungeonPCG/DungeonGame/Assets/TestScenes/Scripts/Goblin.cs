using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goblin : Monster
{

	void Start ()
	{
		this.chaseSpeed = 0.1f;
		this.roamSpeed = 0.05f;

		_animation = GetComponent<Animation> ();

		attackAnimations.Add(_animation["axe attack"].clip);
		attackAnimations.Add(_animation["spear attack"].clip);
	}
	
}
