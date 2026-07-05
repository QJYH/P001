using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorHeavyAttack : HeroHeavyAttack
{
	public override void OnAttackBegin()
	{
		((Warrior)Owner).SetDefendHalfEnable(1);
	}

	public override void OnAttackEnd()
	{
		((Warrior)Owner).SetDefendHalfEnable(0);
		((Warrior)Owner).SetStrikeBackEnable(0);
	}

	public virtual void StrikeBack(WarriorLightAttack lightAttack)
	{
		throw new NotImplementedException();
	}
}
