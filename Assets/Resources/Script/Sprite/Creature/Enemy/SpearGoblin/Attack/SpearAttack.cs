using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : EnemyAttack
{
	protected override void InitAttackHarmValue()
	{
		attackHarmValue = 1f;
	}

	protected override void InitAttackCoolDownMaxTime()
	{
		attackCoolDownMaxTime = 2f;
	}
}
