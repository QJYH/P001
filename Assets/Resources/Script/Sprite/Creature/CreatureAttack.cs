using System;
using UnityEngine;

public class CreatureAttack<OwnerType> : MonoBehaviour where OwnerType : Creature
{
	public HarmInfo CurrentHarmInfo { get; private set; } = new HarmInfo();
	protected OwnerType Owner { get; private set; }

	protected virtual void Awake()
	{
		Owner = GetComponentInParent<OwnerType>();
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
