using System;
using UnityEngine;

public class CreatureAttack : MonoBehaviour
{
	public HarmInfo CurrentHarmInfo { get; private set; } = new HarmInfo();
	protected Creature Owner { get; private set; }

	protected virtual void Awake()
	{
		Owner = GetComponentInParent<Creature>();
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
