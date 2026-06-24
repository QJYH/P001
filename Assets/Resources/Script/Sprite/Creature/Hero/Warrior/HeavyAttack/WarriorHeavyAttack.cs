using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorHeavyAttack<OwnerType> : HeroHeavyAttack<OwnerType> where OwnerType : Warrior
{
	public override void OnAttackBegin()
	{
		Owner.SetDefendHalfEnable(1);
	}

	public override void OnAttackEnd()
	{
		Owner.SetDefendHalfEnable(0);
		Owner.SetStrikeBackEnable(0);
	}

	public virtual void StrikeBack(WarriorLightAttack<Warrior> lightAttack)
	{
		throw new NotImplementedException();
	}
}
