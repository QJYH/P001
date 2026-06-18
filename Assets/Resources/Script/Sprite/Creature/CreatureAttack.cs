using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackRange;

public class CreatureAttack : MonoBehaviour
{
	protected HarmInfo currentHarmInfo = new HarmInfo();

	public HarmInfo GetCurrentHarmInfo()
	{
		return currentHarmInfo;
	}

	public virtual void OnAttackBegin()
	{

	}

	public virtual void OnAttackEnd()
	{

	}

	public virtual void CreateMeleeAttackRange()
	{
		throw new NotImplementedException();
	}

	public virtual void CreateRangedProjectile()
	{
		throw new NotImplementedException();
	}
}
