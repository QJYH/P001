using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public enum EnemyStatus
	{
		Patrol,
		Rest,
		Chase,
		Attack,
		Dead
	}
	private Hero target;
	private EnemyStatus enemyStatus;

	public void SetStatus(EnemyStatus enemyStatus)
	{
		this.enemyStatus = enemyStatus;
	}
	public bool IsPatroling()
	{
		return enemyStatus == EnemyStatus.Patrol;
	}
	public bool IsResting()
	{
		return enemyStatus == EnemyStatus.Rest;
	}
	public bool IsChasing()
	{
		return enemyStatus == EnemyStatus.Chase;
	}
	public bool IsAttacking()
	{
		return enemyStatus == EnemyStatus.Attack;
	}
	public bool IsDead()
	{
		return enemyStatus == EnemyStatus.Dead;
	}

	public void GoToPos(Vector2 value)
	{
		throw new NotImplementedException();
	}
	public bool HaveTarget()
	{
		return target != null;
	}

	public bool HaveTargetInAttackArea()
	{
		throw new NotImplementedException();
	}

	public void TryAttack()
	{
		throw new NotImplementedException();
	}

	public bool IsAttackInCoolDown()
	{
		throw new NotImplementedException();
	}

	
}
