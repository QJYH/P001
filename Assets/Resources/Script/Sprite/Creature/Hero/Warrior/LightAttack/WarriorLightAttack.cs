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
	}

	public virtual void DoFinisher()
	{
		
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
		warrior.SetLightAttackNormalColliderEnable(0);
		warrior.SetLightAttackFinisherColliderEnable(0);
		warrior.SetDefendAllEnable(0);
	}

	public float GetHarmValue(bool isFinisher)
	{
		return (isFinisher) ? finisherHarmValue : normalHarmValue;
	}
}
