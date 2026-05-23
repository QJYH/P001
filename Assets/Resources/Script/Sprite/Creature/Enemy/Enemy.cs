using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

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
	private bool isDetectAreaActive;
	private readonly float maxMoveSpeed = 8f;
	private LinkedList<Vector2> patrolPosList;
	private LinkedListNode<Vector2> currentPatrolPos;

	private void Awake()
	{
		initPatrolPosList();
		SetStatus(EnemyStatus.Patrol);
	}

	private void initPatrolPosList()
	{
		Vector2 currentPos = transform.position;
		Vector2 patrolPos1 = new Vector2(currentPos.x - 1, currentPos.y);
		Vector2 patrolPos2 = new Vector2(currentPos.x + 1, currentPos.y);
		patrolPosList.AddLast(patrolPos1);
		patrolPosList.AddLast(patrolPos2);
		currentPatrolPos = patrolPosList.First;
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		bool isMoving;
		Vector2 targetPos;
		if (IsPatroling())
		{
			isMoving = true;
			float distance = Vector2.Distance(transform.position, currentPatrolPos.Value);
			if (distance < 0.1f)
			{
				currentPatrolPos = currentPatrolPos.Next;
				if (currentPatrolPos == null)
				{
					currentPatrolPos = patrolPosList.First;
				}
			}
			targetPos = currentPatrolPos.Value;
		}
		else if (IsChasing())
		{
			isMoving = true;
			Vector2 heroPos = target.transform.position;
			GameObject enemyDetectArea = transform.Find("EnemyDetectArea").gameObject;
			PolygonCollider2D detectArea = enemyDetectArea.GetComponent<PolygonCollider2D>();
			Vector2 targetPos1 = new Vector2(heroPos.x - detectArea.offset.x, heroPos.y - detectArea.offset.y);
			Vector2 targetPos2 = new Vector2(heroPos.x + detectArea.offset.x, heroPos.y - detectArea.offset.y);
			float distance1 = Vector2.Distance(targetPos1, transform.position);
			float distance2 = Vector2.Distance(targetPos2, transform.position);
			targetPos = (distance1 < distance2) ? targetPos1 : targetPos2;
		}
		else
		{
			isMoving = false;
			targetPos = transform.position;
		}

		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
		if (isMoving)
		{
			rigidbody.velocity = (targetPos - (Vector2)transform.position).normalized * maxMoveSpeed;
			SetForward(rigidbody.velocity.x < 0);
		}
		else
		{
			rigidbody.velocity = Vector2.zero;
		}
	}

	private void SetForward(bool isFacingLeft)
	{

		if (isFacingLeft)
		{
			GetComponent<SpriteRenderer>().flipX = true;
			FlipChildX("EnemyVisionArea", true);
			FlipChildX("EnemyDetectArea", true);
			FlipChildX("EnemyAttackArea", true);
		}
		else
		{
			GetComponent<SpriteRenderer>().flipX = false;
			FlipChildX("EnemyVisionArea", false);
			FlipChildX("EnemyDetectArea", false);
			FlipChildX("EnemyAttackArea", false);
		}
	}
	private void FlipChildX(String childName, bool needFlipX)
	{
		Transform childTrans = transform.Find(childName);
		Vector3 localScale = childTrans.localScale;
		float scaleX = needFlipX ? -MathF.Abs(localScale.x) : MathF.Abs(localScale.x);
		childTrans.localScale = new Vector3(scaleX, localScale.y, localScale.z);
	}

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

	

	public bool IsAttackInCoolDown()
	{
		throw new NotImplementedException();
	}

	public void SetTarget(Hero hero)
	{
		target = hero;
	}

	public void SetDetectAreaActive(bool v)
	{
		isDetectAreaActive = v;
	}
	public bool HaveTarget()
	{
		return target != null;
	}

	public bool HaveTargetInAttackArea()
	{
		return HaveTarget() && isDetectAreaActive;
	}

	public void TryAttack()
	{
		throw new NotImplementedException();
	}

	public void DoHarm(Hero hero)
	{
		throw new NotImplementedException();
	}
}
