using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorLightAttack : HeroLightAttack
{
	protected Warrior warrior;
	public static WarriorLightAttack Create(string attackName, Warrior warrior)
	{
		switch (attackName)
		{
			case nameof(TripleSlash): return new TripleSlash(warrior);
			case nameof(ChargeSlash): return new ChargeSlash(warrior);
			default:
				throw new System.Exception("Unknown Attack Type!!!");
		}
	}

	public virtual void DoFinisher(bool withDefend)
	{
		if (withDefend)
		{
			warrior.SetDefendStatus(true);
		}
	}

	public override void OnAttackEnd()
	{
		warrior.SetDefendStatus(false);
	}
}
