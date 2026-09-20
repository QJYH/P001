using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

public class GnollBone : Projectile
{
	protected override float MaxSpeed { get; } = 12f;
	protected override float MaxAcceleration { get; } = 120f;
	protected override string AttackRangeDirName { get; } = "Prefab/Sprite/Projectile/Enemy/Gnoll/GnollBoneAttackRange";
}
