using CommonEnum;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : Trap
{
	private const float maxExistTime = 5f;

	private Tween explodeTween;

	private void Start()
	{
		explodeTween = DOVirtual.DelayedCall(maxExistTime, () => { Explode(); });
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		AttackRange attackRange = collision.GetComponent<AttackRange>();
		if (attackRange && attackRange.Attacker is Hero)
		{
			Vector3 heroPos = attackRange.Attacker.transform.position;
			Vector2 direction = new Vector2(transform.position.x - heroPos.x, 0);
			BombHit bombHit = gameObject.AddComponent<BombHit>();
			bombHit.Owner = attackRange.Attacker;
			bombHit.TargetType = TargetType.Enemy;
			bombHit.HarmInfo = null;
			bombHit.Direction = direction.normalized;

			GetComponent<Animator>().SetTrigger("Hit");
			explodeTween.Kill();
			Destroy(GetComponent<Bomb>());
		}
	}

	private void Explode()
	{
		CreateExplosion();
		Destroy(gameObject);
	}

	private void CreateExplosion()
	{
		GameObject explosion = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Particle/Explosion/Explosion"));
		explosion.transform.position = transform.position;
		explosion.GetComponent<Explosion>().Owner = null;
		explosion.GetComponent<Explosion>().TargetType = TargetType.Hero;
	}
}
