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

	protected override string GetAttackRangeObjectDirName()
	{
		if (currentPhase == WarriorLightAttackPhase.Normal)
		{
			return GetNormalAttackRangeObjectDirName();
		}
		else if (currentPhase == WarriorLightAttackPhase.Finisher)
		{
			return GetFinisherAttackRangeObjectDirName();
		}
		else
		{
			throw new Exception("Error Light Attack Phase!");
		}
	}

	protected virtual string GetFinisherAttackRangeObjectDirName()
	{
		throw new NotImplementedException();
	}

	protected virtual string GetNormalAttackRangeObjectDirName()
	{
		throw new NotImplementedException();
	}

	protected override float GetAttackRangeDurationTime()
	{
		if (currentPhase == WarriorLightAttackPhase.Normal)
		{
			return GetNormalAttackRangeDurationTime();
		}
		else if (currentPhase == WarriorLightAttackPhase.Finisher)
		{
			return GetFinisherAttackRangeDurationTime();
		}
		else
		{
			throw new Exception("Error Light Attack Phase!");
		}
	}

	protected virtual float GetFinisherAttackRangeDurationTime()
	{
		throw new NotImplementedException();
	}

	protected virtual float GetNormalAttackRangeDurationTime()
	{
		throw new NotImplementedException();
	}

	protected override Creature GetAttacker()
	{
		return warrior;
	}
}
