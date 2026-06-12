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
	private Creature attacker;
	private HarmInfo harmInfo;
	private TargetType targetType;
	private float durationTime;

	private void SetAttacker(Creature attacker)
	{
		this.attacker = attacker;
	}
	private void SetHarmInfo(HarmInfo harmInfo)
	{
		this.harmInfo = harmInfo;
	}
	private void SetTargetType(TargetType targetType)
	{
		this.targetType = targetType;
	}
	private void SetDurationTime(float durationTime)
	{
		this.durationTime = durationTime;
		if (durationTime > 0)
		{
			StartCoroutine(DelayDestory(durationTime));
		}
	}

	private IEnumerator DelayDestory(float durationTime)
	{
		yield return new WaitForSeconds(durationTime);

		GameObject.Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (targetType == TargetType.Hero)
		{
			Hero receiver = collision.GetComponent<Hero>();
			if (receiver == null) return;
			Creature.HandleAttack(attacker, receiver, harmInfo);
		}
		else if (targetType == TargetType.Enemy)
		{
			Enemy receiver = collision.GetComponent<Enemy>();
			if (receiver == null) return;
			Creature.HandleAttack(attacker, receiver, harmInfo);
		}
	}

	public static GameObject CreateAttackRangeObject(string attackRangeDirName, Transform parentTransform, Creature attacker, HarmInfo harmInfo, TargetType targetType, float durationTime)
	{
		GameObject attackRangeObject = Instantiate(Resources.Load<GameObject>(attackRangeDirName), parentTransform);
		attackRangeObject.GetComponent<AttackRange>().SetAttacker(attacker);
		attackRangeObject.GetComponent<AttackRange>().SetHarmInfo(harmInfo);
		attackRangeObject.GetComponent<AttackRange>().SetTargetType(targetType);
		attackRangeObject.GetComponent<AttackRange>().SetDurationTime(durationTime);

		return attackRangeObject;
	}


}
