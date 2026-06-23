using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

public class AttackRange : Range
{
	private Creature Attacker { get; set; }
	private HarmInfo HarmInfo { get; set; }
	private AttackTargetType TargetType { get; set; }
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
		Creature receiver = null;
		if (TargetType == AttackTargetType.Hero)
		{
			receiver = collision.GetComponent<Hero>();
			if (receiver == null) return;
		}
		else if (TargetType == AttackTargetType.Enemy)
		{
			receiver = collision.GetComponent<Enemy>();
			if (receiver == null) return;
		}
		Creature.HandleAttack(Attacker, receiver, HarmInfo);
	}

	public static GameObject CreateAttackRangeObject(string attackRangeDirName, Creature attacker, HarmInfo harmInfo, AttackTargetType targetType, float durationTime)
	{
		GameObject attackRangeObject = Instantiate(Resources.Load<GameObject>(attackRangeDirName));
		attackRangeObject.GetComponent<AttackRange>().Attacker = attacker;
		attackRangeObject.GetComponent<AttackRange>().HarmInfo = harmInfo;
		attackRangeObject.GetComponent<AttackRange>().TargetType = targetType;
		attackRangeObject.GetComponent<AttackRange>().DurationTime = durationTime;

		return attackRangeObject;
	}


}
