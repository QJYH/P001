using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorLightAttack : HeroLightAttack
{
	protected Warrior warrior;
	protected float normalHarmValue;
	protected float finisherHarmValue;

	protected virtual void Awake()
	{
		warrior = GetComponentInParent<Warrior>();
	}

	public virtual void DoFinisher(bool withDefend)
	{
		if (withDefend)
		{
			warrior.SetDefendStatus(true);
		}
	}

	public override void OnAttackEnd()
	{
		warrior.SetDefendStatus(false);
	}

	public float GetHarmValue(bool isFinisher)
	{
		return (isFinisher) ? finisherHarmValue : normalHarmValue;
	}
}
