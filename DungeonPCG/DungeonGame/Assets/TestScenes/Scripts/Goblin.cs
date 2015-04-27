﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goblin : Monster
{
	void Start ()
	{
		this.aggroRange = AggroRange.Low;
		this.roamSpeed = Speed.Low;
		this.chaseSpeed = Speed.Medium;
		this.attackType = AttackType.Melee;
		this.maxHealth = Health.Low;
		this.currentHealth = Health.Low;

		attackAnimations.Add(_animation["axe attack"].clip);
		attackAnimations.Add(_animation["spear attack"].clip);

	}
	
}
