using CommonEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArcherArrow : Projectile
{
	private const string archerArrowAttackRangeDirName = "Prefab/Sprite/Projectile/Enemy/Archer/ArcherArrowAttackRange";
	private const float archerArrowAttackRangeDurationTime = 0.1f;

	protected override float MaxSpeed { get; set; } = 12f;
	protected override float MaxAcceleration { get; set; } = 120f;
	protected override float AngleSpeed { get; set; } = 210f;


	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		Rotate();
	}

	private void Rotate()
	{
		float rotateAngle = Vector2.SignedAngle(Vector2.right, Direction);
		transform.rotation = Quaternion.Euler(0, 0, rotateAngle);
	}

	protected override void CreateProjectileAttackRange()
	{
		string attackRangeDirName = archerArrowAttackRangeDirName;
		Creature attacker = Owner;
		TargetType targetType = TargetType;
		float attackRangeDurationTime = archerArrowAttackRangeDurationTime;

		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, HarmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.position = transform.position;
	}
}
