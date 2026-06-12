using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroAttack : CreatureAttack
{
	protected override AttackRange.TargetType GetAttackRangeTargetType()
	{
		return AttackRange.TargetType.Enemy;
	}
}
