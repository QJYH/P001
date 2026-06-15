using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static WarriorLightAttack;

public class EnemyAttack : CreatureAttack
{
	protected Enemy enemy;
	private bool isTargetInDectectRange = false;
	private float attackCoolDownTime = 0;
	protected float attackCoolDownMaxTime;
	protected float attackHarmValue;
	private void Awake()
	{
		enemy = GetComponentInParent<Enemy>();
		InitAttackCoolDownMaxTime();
		InitAttackHarmValue();
	}

	private void Update()
	{
		UpdateCoolDownTime();
	}

	protected virtual void InitAttackHarmValue()
	{
		throw new NotImplementedException();
	}

	protected virtual void InitAttackCoolDownMaxTime()
	{
		throw new NotImplementedException();
	}

	private void UpdateCoolDownTime()
	{
		if (attackCoolDownTime <= 0) return;
		attackCoolDownTime -= Time.deltaTime;
	}

	public bool IsAttackInCoolDown()
	{
		return attackCoolDownTime > 0;
	}

	public void OnTargetEnterDectectRange()
	{
		isTargetInDectectRange = true;
	}

	public void OnTargetExitDectectRange()
	{
		isTargetInDectectRange = false;
	}

	private bool IsTargetInDectectRange()
	{
		return isTargetInDectectRange;
	}
	public bool CanLaunchAttack()
	{
		return enemy.HaveTarget() && IsTargetInDectectRange();
	}

	public void Attack()
	{
		currentHarmInfo.Rsset();
		currentHarmInfo.SetBaseHarm(attackHarmValue);
		attackCoolDownTime = attackCoolDownMaxTime;
		enemy.Attack();
	}

	public override void OnAttackBegin()
	{
		
	}

	public override void OnAttackEnd()
	{

	}

}
