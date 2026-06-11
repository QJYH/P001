using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class WarriorLightAttack : HeroLightAttack
{
	protected Warrior warrior;
	protected float normalHarmValue;
	protected float finisherHarmValue;
	public enum LightAttackPhase
	{
		None,
		Normal,
		Finisher
	}
	protected LightAttackPhase currentPhase;

	protected virtual void Awake()
	{
		warrior = GetComponentInParent<Warrior>();
		currentPhase = LightAttackPhase.None;
		transform.Find("NormalAttackRange").GetComponent<AttackRange>().SetTargetType(AttackRange.TargetType.Enemy);
		transform.Find("FinisherAttackRange").GetComponent<AttackRange>().SetTargetType(AttackRange.TargetType.Enemy);
	}

	public virtual void DoNormal()
	{
		currentHarmInfo.Rsset();
		currentHarmInfo.SetBaseHarm(normalHarmValue);
		currentPhase = LightAttackPhase.Normal;
		warrior.GetComponent<Animator>().SetInteger("LightAttackPhase", 1);
		warrior.GetComponent<Animator>().SetTrigger("LightAttack");
	}

	public virtual void DoFinisher(float harmRate)
	{
		currentHarmInfo.Rsset();
		currentHarmInfo.SetBaseHarm(finisherHarmValue);
		currentHarmInfo.AddHarmRate(harmRate);
		currentPhase = LightAttackPhase.Finisher;
		warrior.GetComponent<Animator>().SetInteger("LightAttackPhase", 2);
		warrior.GetComponent<Animator>().SetTrigger("LightAttack");
	}

	public override void OnAttackBegin()
	{
		if (currentPhase == LightAttackPhase.Finisher)
		{
			warrior.SetDefendAllEnable(1);
		}
	}

	public override void OnAttackEnd()
	{
		currentPhase = LightAttackPhase.None;
		SetAttackRangeEnable("NormalAttackRange", false);
		SetAttackRangeEnable("FinisherAttackRange", false);
		warrior.SetDefendAllEnable(0);
	}
}
