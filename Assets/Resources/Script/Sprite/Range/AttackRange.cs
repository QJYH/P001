using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : Range
{
	public enum TargetType
	{
		Hero,
		Enemy
	}
	private TargetType targetType;
	private HarmInfo harmInfo;

	public void SetTargetType(TargetType targetType)
	{
		this.targetType = targetType;
	}

	public void SetHarmInfo(HarmInfo harmInfo)
	{
		this.harmInfo = harmInfo;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (targetType == TargetType.Hero)
		{
			Hero receiver = collision.GetComponent<Hero>();
			if (receiver == null) return;
			Creature.HandleAttack(receiver, harmInfo);
		}
		else if (targetType == TargetType.Enemy)
		{
			Enemy receiver = collision.GetComponent<Enemy>();
			if (receiver == null) return;
			Creature.HandleAttack(receiver, harmInfo);
		}
	}

	public void EnableAttackRange(HarmInfo harmInfo)
	{
		this.harmInfo = harmInfo;
		GetComponent<PolygonCollider2D>().enabled = true;
	}

	public void DisableAttackRange()
	{
		GetComponent<PolygonCollider2D>().enabled = false;
	}
}
