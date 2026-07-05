using CommonEnum;
using System;
using UnityEngine;

public class WarriorLightAttack : HeroLightAttack
{
	protected virtual float NormalHarmValue { get; } = 0;
	protected virtual float FinisherHarmValue { get; } = 0;
	protected virtual string NormalAttackRangeObjectDirName { get; } = "";
	protected virtual string FinisherAttackRangeObjectDirName { get; } = "";
	protected virtual float NormalAttackRangeDurationTime { get; } = 0;
	protected virtual float FinisherAttackRangeDurationTime { get; } = 0;

	private enum WarriorLightAttackPhase
	{
		None,
		Normal,
		Finisher
	}
	private WarriorLightAttackPhase currentPhase;

	protected override void Awake()
	{
		base.Awake();
		currentPhase = WarriorLightAttackPhase.None;
		
	}

	public virtual void DoNormal()
	{
		CurrentHarmInfo.Rsset();
		CurrentHarmInfo.BaseHarm = NormalHarmValue;
		currentPhase = WarriorLightAttackPhase.Normal;
		Owner.GetComponent<Animator>().SetInteger("LightAttackPhase", 1);
		Owner.GetComponent<Animator>().SetTrigger("LightAttack");
	}

	public virtual void DoFinisher(float harmRate)
	{
		CurrentHarmInfo.Rsset();
		CurrentHarmInfo.BaseHarm = FinisherHarmValue;
		CurrentHarmInfo.AddHarmRate(harmRate);
		currentPhase = WarriorLightAttackPhase.Finisher;
		Owner.GetComponent<Animator>().SetInteger("LightAttackPhase", 2);
		Owner.GetComponent<Animator>().SetTrigger("LightAttack");
	}

	public override void OnAttackBegin()
	{
		if (currentPhase == WarriorLightAttackPhase.Finisher)
		{
			((Warrior)Owner).SetDefendAllEnable(1);
		}
	}

	public override void OnAttackEnd()
	{
		currentPhase = WarriorLightAttackPhase.None;
		((Warrior)Owner).SetDefendAllEnable(0);
	}

	public override void CreateMeleeAttackRange()
	{
		string attackRangeDirName;
		float attackRangeDurationTime;
		Creature attacker = Owner;
		HarmInfo harmInfo = new HarmInfo(CurrentHarmInfo);
		TargetType targetType = TargetType.Enemy;


		if (currentPhase == WarriorLightAttackPhase.Normal)
		{
			attackRangeDirName = NormalAttackRangeObjectDirName;
			attackRangeDurationTime = NormalAttackRangeDurationTime;
		}
		else if (currentPhase == WarriorLightAttackPhase.Finisher)
		{
			attackRangeDirName = FinisherAttackRangeObjectDirName;
			attackRangeDurationTime = FinisherAttackRangeDurationTime;
		}
		else
		{
			throw new Exception("Error Light Attack Phase!");
		}
		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, harmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.SetParent(transform, false);
	}
}
