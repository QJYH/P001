using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroAttack : Object
{
	public static HeroAttack CreateHeroAttack(string name, Hero hero)
	{
		switch (name)
		{
			//WarriorLightAttack
			case nameof(TripleSlash): return new TripleSlash((Warrior)hero);
			case nameof(ChargeSlash): return new ChargeSlash((Warrior)hero);
			//WarriorHeavyAttack
			case nameof(NormalGuard): return new NormalGuard((Warrior)hero);
			default:
				throw new System.Exception("Unknown Attack Type!!!");
		}
	}
}
