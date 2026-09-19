using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : EnemyAttack
{
	private const float spiderAttackRangeDurationTime = 0.2f;
	private const string spiderAttackRangeDirName = "Prefab/Sprite/Creature/Enemy/Spider/SpiderAttackRange";

	protected override float AttackCoolDownMaxTime { get; } = 2f;
	protected override float AttackHarmValue { get; } = 2f;

	public override void CreateMeleeAttackRange()
	{
		string attackRangeDirName = spiderAttackRangeDirName;
		Creature attacker = Owner;
		HarmInfo harmInfo = new HarmInfo(CurrentHarmInfo);
		TargetType targetType = TargetType.Hero;
		float attackRangeDurationTime = spiderAttackRangeDurationTime;

		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, harmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.SetParent(transform, false);
	}
}
