using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : EnemyAttack
{
	private readonly float spearAttackHarmValue = 1f;
	private readonly float spearAttackCoolDownMaxTime = 2f;
	private readonly float spearAttackRangeDurationTime = 0.2f;
	private readonly string spearAttackRangeDirName = "Prefab/Sprite/Creature/Enemy/SpearGoblin/SpearAttackRange";
	protected override void InitAttackHarmValue()
	{
		attackHarmValue = spearAttackHarmValue;
	}

	protected override void InitAttackCoolDownMaxTime()
	{
		attackCoolDownMaxTime = spearAttackCoolDownMaxTime;
	}

	public override void CreateMeleeAttackRange()
	{
		string attackRangeDirName = spearAttackRangeDirName;
		Creature attacker = enemy;
		HarmInfo harmInfo = new HarmInfo(currentHarmInfo);
		AttackRange.TargetType targetType = AttackRange.TargetType.Hero;
		float attackRangeDurationTime = spearAttackRangeDurationTime;

		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, harmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.SetParent(transform);
	}
}
