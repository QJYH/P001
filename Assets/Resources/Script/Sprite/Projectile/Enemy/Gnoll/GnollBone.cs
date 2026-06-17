using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnollBone : Projectile
{
	private readonly float gnollBoneMaxSpeed = 5f;
	private readonly string gnollBoneAttackRangeDirName = "Prefab/Sprite/Projectile/Enemy/Gnoll/GnollBoneAttackRange";
	private readonly float gnollBoneAttackRangeDurationTime = 0;
	protected override void InitMaxSpeed()
	{
		maxSpeed = gnollBoneMaxSpeed;
	}
	protected override GameObject CreateProjectileAttackRange()
	{
		string attackRangeDirName = gnollBoneAttackRangeDirName;
		Transform parentTransform = transform;
		Creature attacker = owner;
		AttackRange.TargetType targetType = AttackRange.TargetType.Hero;
		float attackRangeDurationTime = gnollBoneAttackRangeDurationTime;

		return AttackRange.CreateAttackRangeObject(attackRangeDirName, parentTransform, attacker, harmInfo, targetType, attackRangeDurationTime);
	}
}
