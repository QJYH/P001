using System;
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

	public override void OnAttackBegin()
	{
		warrior.SetDefendHalfEnable(1);
	}

	public override void OnAttackEnd()
	{
		warrior.SetDefendHalfEnable(0);
		warrior.SetStrikeBackEnable(0);
	}

	public virtual void StrikeBack(WarriorLightAttack lightAttack)
	{
		throw new NotImplementedException();
	}
}
