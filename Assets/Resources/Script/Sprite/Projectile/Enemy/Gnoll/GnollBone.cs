using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnollBone : Projectile
{
	private readonly float gnollBoneMaxSpeed = 5f;
	private readonly string gnollBoneAttackRangeDirName = "Prefab/Sprite/Projectile/Enemy/Gnoll/GnollBoneAttackRange";
	private readonly float gnollBoneAttackRangeDurationTime = 0.1f;
	protected override void InitMaxSpeed()
	{
		maxSpeed = gnollBoneMaxSpeed;
	}
	protected override void CreateProjectileAttackRange()
	{
		string attackRangeDirName = gnollBoneAttackRangeDirName;
		Creature attacker = owner;
		AttackRange.TargetType targetType = AttackRange.TargetType.Hero;
		float attackRangeDurationTime = gnollBoneAttackRangeDurationTime;

		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, harmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.position = transform.position;
	}
}
