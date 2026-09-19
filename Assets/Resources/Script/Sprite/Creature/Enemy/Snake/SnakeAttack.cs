using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAttack : EnemyAttack
{
	private const float snakeAttackRangeDurationTime = 0.2f;
	private const string snakeAttackRangeDirName = "Prefab/Sprite/Creature/Enemy/Snake/SnakeAttackRange";

	protected override float AttackCoolDownMaxTime { get; } = 2f;
	protected override float AttackHarmValue { get; } = 2f;

	public override void CreateMeleeAttackRange()
	{
		string attackRangeDirName = snakeAttackRangeDirName;
		Creature attacker = Owner;
		HarmInfo harmInfo = new HarmInfo(CurrentHarmInfo);
		TargetType targetType = TargetType.Hero;
		float attackRangeDurationTime = snakeAttackRangeDurationTime;

		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, harmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.SetParent(transform, false);
	}
}
