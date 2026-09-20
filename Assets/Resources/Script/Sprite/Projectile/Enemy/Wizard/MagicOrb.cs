using CommonEnum;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicOrb : Projectile
{
	protected override float MaxSpeed { get; } = 3f;
	protected override float MaxAcceleration { get; } = 30f;
	protected override float AngleSpeed { get; } = 180f;
	protected override float CollisionDestroyDelayTime { get; } = 2f;
	protected override bool CanBeStruckBack { get; } = false;
	protected override string AttackRangeDirName { get; } = "Prefab/Sprite/Projectile/Enemy/Wizard/MagicOrbAttackRange";

	protected override void HitTarget()
	{
		GetComponent<Animator>().SetTrigger("Explode");
		base.HitTarget();
	}

	protected override void HitEnvironmentBlock()
	{
		GetComponent<Animator>().SetTrigger("Explode");
		base.HitEnvironmentBlock();
	}
}
