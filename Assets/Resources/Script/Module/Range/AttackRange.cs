using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

public class AttackRange : Range
{
	private Creature Attacker { get; set; }
	private HarmInfo HarmInfo { get; set; }
	private TargetType TargetType { get; set; }
	private float DurationTime { get; set; } = 0;

private void Start()
	{
		TryDestory();
	}

	private void TryDestory()
	{
		if (DurationTime > 0)
		{
			StartCoroutine(DelayDestory(DurationTime));
		}
	}
	private IEnumerator DelayDestory(float durationTime)
	{
		yield return new WaitForSeconds(durationTime);

		GameObject.Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Projectile>())
		{
			Projectile projectile = collision.GetComponent<Projectile>();
			TryStrikeProjectileBack(projectile);
			return;
		}

		Creature receiver = null;
		if (TargetType == TargetType.Hero)
		{
			receiver = collision.GetComponent<Hero>();
			if (receiver == null) return;
		}
		else if (TargetType == TargetType.Enemy)
		{
			receiver = collision.GetComponent<Enemy>();
			if (receiver == null) return;
		}
		Creature.HandleAttack(Attacker, receiver, HarmInfo);
	}

	private void TryStrikeProjectileBack(Projectile projectile)
	{
		if ((Attacker is Hero) && (TargetType == TargetType.Enemy) && (projectile.Owner is Enemy))
		{
			projectile.OnHeroStrikeBack(Attacker);
		}
	}

	public static GameObject CreateAttackRangeObject(string attackRangeDirName, Creature attacker, HarmInfo harmInfo, TargetType targetType, float durationTime)
	{
		GameObject attackRangeObject = Instantiate(Resources.Load<GameObject>(attackRangeDirName));
		attackRangeObject.GetComponent<AttackRange>().Attacker = attacker;
		attackRangeObject.GetComponent<AttackRange>().HarmInfo = harmInfo;
		attackRangeObject.GetComponent<AttackRange>().TargetType = targetType;
		attackRangeObject.GetComponent<AttackRange>().DurationTime = durationTime;

		return attackRangeObject;
	}


}
