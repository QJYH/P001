using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHit : Projectile
{
	protected override float MaxSpeed { get; set; } = 10f;
	protected override float MaxAcceleration { get; set; } = 100f;

	protected override void CreateProjectileAttackRange()
	{
		CreateExplosion();
	}

	private void CreateExplosion()
	{
		GameObject explosion = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Particle/Explosion/Explosion"));
		explosion.transform.position = transform.position;
		explosion.GetComponent<Explosion>().Owner = Owner;
		explosion.GetComponent<Explosion>().TargetType = TargetType;
	}
}
