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
	private float durationTime = 0;

	private void Start()
	{
		TryDestory();
	}

	private void TryDestory()
	{
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

	public void SetAttacker(Creature attacker)
	{
		this.attacker = attacker;
	}
	public void SetHarmInfo(HarmInfo harmInfo)
	{
		this.harmInfo = harmInfo;
	}
	public void SetTargetType(TargetType targetType)
	{
		this.targetType = targetType;
	}
	public void SetDurationTime(float durationTime)
	{
		this.durationTime = durationTime;
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Creature receiver = null;
		if (targetType == TargetType.Hero)
		{
			receiver = collision.GetComponent<Hero>();
			if (receiver == null) return;
		}
		else if (targetType == TargetType.Enemy)
		{
			receiver = collision.GetComponent<Enemy>();
			if (receiver == null) return;
		}
		Creature.HandleAttack(attacker, receiver, harmInfo);
	}

	public static GameObject CreateAttackRangeObject(string attackRangeDirName, Creature attacker, HarmInfo harmInfo, TargetType targetType, float durationTime)
	{
		GameObject attackRangeObject = Instantiate(Resources.Load<GameObject>(attackRangeDirName));
		attackRangeObject.GetComponent<AttackRange>().SetAttacker(attacker);
		attackRangeObject.GetComponent<AttackRange>().SetHarmInfo(harmInfo);
		attackRangeObject.GetComponent<AttackRange>().SetTargetType(targetType);
		attackRangeObject.GetComponent<AttackRange>().SetDurationTime(durationTime);

		return attackRangeObject;
	}


}
