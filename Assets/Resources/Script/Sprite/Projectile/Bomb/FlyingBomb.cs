using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBomb : Projectile
{
	protected override float MaxSpeed { get; } = 10f;
	protected override float MaxAcceleration { get; } = 100f;

	protected override void HitTarget()
	{
		CreateExplosion();
		GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", Vector2.zero);
		DelayDestory(CollisionDestroyDelayTime);
		Target = null;
	}

	private void CreateExplosion()
	{
		GameObject explosion = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Particle/Explosion/Explosion"));
		explosion.transform.position = transform.position;
		explosion.GetComponent<Explosion>().Owner = Owner;
		explosion.GetComponent<Explosion>().TargetType = TargetType;
	}
}
