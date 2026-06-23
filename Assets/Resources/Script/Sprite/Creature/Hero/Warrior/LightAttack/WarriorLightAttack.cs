using CommonEnum;
using System;
using UnityEngine;

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
		CurrentHarmInfo.Rsset();
		CurrentHarmInfo.BaseHarm = normalHarmValue;
		currentPhase = WarriorLightAttackPhase.Normal;
		warrior.GetComponent<Animator>().SetInteger("LightAttackPhase", 1);
		warrior.GetComponent<Animator>().SetTrigger("LightAttack");
	}

	public virtual void DoFinisher(float harmRate)
	{
		CurrentHarmInfo.Rsset();
		CurrentHarmInfo.BaseHarm = finisherHarmValue;
		CurrentHarmInfo.AddHarmRate(harmRate);
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

	public override void CreateMeleeAttackRange()
	{
		string attackRangeDirName;
		float attackRangeDurationTime;
		Creature attacker = warrior;
		HarmInfo harmInfo = new HarmInfo(CurrentHarmInfo);
		AttackTargetType targetType = AttackTargetType.Enemy;


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
		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, harmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.SetParent(transform);
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
