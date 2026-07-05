using System;
using UnityEngine;

public class EnemyAttack : CreatureAttack
{
	protected virtual float AttackCoolDownMaxTime { get; } = 0;
	protected virtual float AttackHarmValue { get; } = 0;

	private bool isTargetInDectectRange = false;
	private float attackCoolDownTime = 0;

	protected override void Awake()
	{
		base.Awake();
	}

	private void Update()
	{
		UpdateCoolDownTime();
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
		return ((Enemy)Owner).HaveTarget() && IsTargetInDectectRange();
	}

	public void Attack()
	{
		CurrentHarmInfo.Rsset();
		CurrentHarmInfo.BaseHarm = AttackHarmValue;
		attackCoolDownTime = AttackCoolDownMaxTime;
		((Enemy)Owner).Attack();
	}
}
