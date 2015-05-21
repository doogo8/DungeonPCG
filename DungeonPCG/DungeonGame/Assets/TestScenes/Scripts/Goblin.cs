using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProD{
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
			this.attackDamage = Damage.Low;
			this.coinReward = 3;

			attackAnimations.Add(_animation["axe attack"].clip);
			attackAnimations.Add(_animation["spear attack"].clip);
		}
	}
}
