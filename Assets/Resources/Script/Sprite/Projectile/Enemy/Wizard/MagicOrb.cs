using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicOrb : Projectile
{
	private const string magicOrbAttackRangeDirName = "Prefab/Sprite/Projectile/Enemy/Wizard/MagicOrbAttackRange";
	private const float magicOrbAttackRangeDurationTime = 0.1f;

	protected override float MaxSpeed { get; set; } = 3f;
	protected override float MaxAcceleration { get; set; } = 30f;
	protected override float AngleSpeed { get; set; } = 180f;

	protected override void CreateProjectileAttackRange()
	{
		string attackRangeDirName = magicOrbAttackRangeDirName;
		Creature attacker = Owner;
		TargetType targetType = TargetType;
		float attackRangeDurationTime = magicOrbAttackRangeDurationTime;

		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, HarmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.position = transform.position;
	}
}
