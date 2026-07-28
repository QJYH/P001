using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

public class GnollBone : Projectile
{
	private const string gnollBoneAttackRangeDirName = "Prefab/Sprite/Projectile/Enemy/Gnoll/GnollBoneAttackRange";
	private const float gnollBoneAttackRangeDurationTime = 0.1f;

	protected override float MaxSpeed { get; set; } = 10f;
	protected override float MaxAcceleration { get; set; } = 100f;

	protected override void CreateProjectileAttackRange()
	{
		string attackRangeDirName = gnollBoneAttackRangeDirName;
		Creature attacker = Owner;
		TargetType targetType = TargetType;
		float attackRangeDurationTime = gnollBoneAttackRangeDurationTime;

		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, HarmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.position = transform.position;
	}
}
