using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorHeavyAttack : HeroHeavyAttack
{
	public static WarriorHeavyAttack Create(string attackName, Warrior warrior)
	{
		switch (attackName)
		{
			case nameof(NormalGuard): return new NormalGuard(warrior);
			default:
				throw new System.Exception("Unknown Attack Type!!!");
		}
	}
}
