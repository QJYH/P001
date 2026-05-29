using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorHeavyAttack : HeroHeavyAttack
{
	protected Warrior warrior;

	private void Awake()
	{
		warrior = GetComponentInParent<Warrior>();
	}
}
