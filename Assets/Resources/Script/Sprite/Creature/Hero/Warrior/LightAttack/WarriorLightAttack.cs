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
	public enum WarriorLightAttackPhase
	{
		None,
		Normal,
		Finisher
	}
	protected WarriorLightAttackPhase currentPhase;

	protected virtual void Awake()
	{
		warrior = GetComponentInParent<Warrior>();
		currentPhase = WarriorLightAttackPhase.None;
	}

	public virtual void DoNormal()
	{
		currentHarmInfo.Rsset();
		currentHarmInfo.SetBaseHarm(normalHarmValue);
		currentPhase = WarriorLightAttackPhase.Normal;
		warrior.GetComponent<Animator>().SetInteger("LightAttackPhase", 1);
		warrior.GetComponent<Animator>().SetTrigger("LightAttack");
	}

	public virtual void DoFinisher(float harmRate)
	{
		currentHarmInfo.Rsset();
		currentHarmInfo.SetBaseHarm(finisherHarmValue);
		currentHarmInfo.AddHarmRate(harmRate);
		currentPhase = WarriorLightAttackPhase.Finisher;
		warrior.GetComponent<Animator>().SetInteger("LightAttackPhase", 2);
		warrior.GetComponent<Animator>().SetTrigger("LightAttack");
	}

	public override void OnAttackBegin()
	{
		if (currentPhase == WarriorLightAttackPhase.Finisher)
		{
			warrior.SetDefendAllEnable(1);
		}
	}

	public override void OnAttackEnd()
	{
		currentPhase = WarriorLightAttackPhase.None;
		warrior.SetDefendAllEnable(0);
	}

	public override GameObject CreateAttackRange()
	{
		string attackRangeDirName;
		float attackRangeDurationTime;
		Transform parentTransform = transform;
		Creature attacker = warrior;
		HarmInfo harmInfo = new HarmInfo(currentHarmInfo);
		AttackRange.TargetType targetType = AttackRange.TargetType.Enemy;


		if (currentPhase == WarriorLightAttackPhase.Normal)
		{
			attackRangeDirName = GetNormalAttackRangeObjectDirName();
			attackRangeDurationTime = GetNormalAttackRangeDurationTime();
		}
		else if (currentPhase == WarriorLightAttackPhase.Finisher)
		{
			attackRangeDirName = GetFinisherAttackRangeObjectDirName();
			attackRangeDurationTime = GetFinisherAttackRangeDurationTime();
		}
		else
		{
			throw new Exception("Error Light Attack Phase!");
		}
		return AttackRange.CreateAttackRangeObject(attackRangeDirName, parentTransform, attacker, harmInfo, targetType, attackRangeDurationTime);
	}

	

	protected virtual string GetFinisherAttackRangeObjectDirName()
	{
		throw new NotImplementedException();
	}

	protected virtual string GetNormalAttackRangeObjectDirName()
	{
		throw new NotImplementedException();
	}

	protected virtual float GetFinisherAttackRangeDurationTime()
	{
		throw new NotImplementedException();
	}

	protected virtual float GetNormalAttackRangeDurationTime()
	{
		throw new NotImplementedException();
	}
}
