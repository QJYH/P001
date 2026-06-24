using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;
public class SpearAttack : EnemyAttack<SpearGoblin>
{
	private const float spearAttackRangeDurationTime = 0.2f;
	private const string spearAttackRangeDirName = "Prefab/Sprite/Creature/Enemy/SpearGoblin/SpearAttackRange";

	protected override float AttackCoolDownMaxTime { get; } = 2f;
	protected override float AttackHarmValue { get; } = 1f;

	public override void CreateMeleeAttackRange()
	{
		string attackRangeDirName = spearAttackRangeDirName;
		Creature attacker = Owner;
		HarmInfo harmInfo = new HarmInfo(CurrentHarmInfo);
		TargetType targetType = TargetType.Hero;
		float attackRangeDurationTime = spearAttackRangeDurationTime;

		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, harmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.SetParent(transform);
	}
}
