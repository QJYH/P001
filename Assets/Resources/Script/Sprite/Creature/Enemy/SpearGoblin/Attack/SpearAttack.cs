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

	protected override string GetAttackRangeObjectDirName()
	{
		return "Prefab/Sprite/Creature/Enemy/SpearGoblin/SpearAttackRange";
	}

	protected override float GetAttackRangeDurationTime()
	{
		return 0.2f;
	}
}
