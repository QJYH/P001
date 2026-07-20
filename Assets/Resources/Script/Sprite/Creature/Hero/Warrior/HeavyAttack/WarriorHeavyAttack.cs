using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorHeavyAttack : HeroHeavyAttack
{
	public override void OnAttackBegin()
	{
		((Warrior)Owner).DefendEnable = true;
	}

	public override void OnAttackEnd()
	{
		((Warrior)Owner).DefendEnable = false;
		((Warrior)Owner).StrikeBackEnable = false;
	}

	public virtual void StrikeBack(WarriorLightAttack lightAttack)
	{
		throw new NotImplementedException();
	}
}
