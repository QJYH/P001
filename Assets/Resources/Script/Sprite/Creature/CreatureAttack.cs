using System;
using UnityEngine;

public class CreatureAttack : MonoBehaviour
{
	public HarmInfo CurrentHarmInfo { get; private set; } = new HarmInfo();

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
