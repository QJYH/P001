using CommonEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArcherArrow : Projectile
{
	protected override float MaxSpeed { get; } = 12f;
	protected override float MaxAcceleration { get; } = 120f;
	protected override float AngleSpeed { get; } = 210f;
	protected override string AttackRangeDirName { get; } = "Prefab/Sprite/Projectile/Enemy/Archer/ArcherArrowAttackRange";

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
}
