using CommonEnum;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private const float collisionDestroyDelayTime = 0.3f;
	private const float maxExistTime = 5f;
	private enum Status
	{
		Running,
		ToBeDestory
	}

	public Creature Owner { get; set; }
	public GameObject Target { get; set; }
	public TargetType TargetType { get; set; }
	public HarmInfo HarmInfo { get; set; }
	public Vector2 Direction { get; set; }
	protected virtual float MaxSpeed { get; set; } = 0;
	protected virtual float MaxAcceleration { get; set; } = 0;
	protected virtual float AngleSpeed { get; set; } = 0;
	private Status currentStatus;
	private Sequence delayDestorySequence;

	private void Start()
	{
		currentStatus = Status.Running;
		AddBaseMoveVelocity();
		DelayDestory(maxExistTime);
	}

	private void OnDestroy()
	{
		delayDestorySequence?.Kill();
	}

	protected virtual void FixedUpdate()
	{
		TryFollowTarget();
	}

	private void AddBaseMoveVelocity()
	{
		Vector2 targetVelocity = Direction * MaxSpeed;
		Velocity velocity = new Velocity(Vector2.zero, targetVelocity, MaxAcceleration);
		GetComponent<MoveManager>().AddVelocity("_BaseMove", velocity);
	}

	private void TryFollowTarget()
	{
		if (currentStatus == Status.ToBeDestory) return;
		if (Target == null) return;

		Vector2 targetDirection = (Target.transform.position - transform.position).normalized;
		Direction = Vector3.RotateTowards(Direction, targetDirection, Time.deltaTime * AngleSpeed * Mathf.Deg2Rad, 0);
		Velocity baseMove = GetComponent<MoveManager>().GetVelocity("_BaseMove");
		GetComponent<MoveManager>().ChangeCurrentVelocity("_BaseMove", baseMove.CurrentVelocity.magnitude * Direction);
		GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", MaxSpeed * Direction);

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		AttackRange attackRange = collision.GetComponent<AttackRange>();
		if (attackRange)
		{
			if ((attackRange.Attacker is Hero) && (attackRange.TargetType == TargetType.Enemy) && (Owner is Enemy))
			{
				OnHeroStrikeBack(attackRange.Attacker);
			}
			return;
		}

		if (LayerMask.LayerToName(collision.gameObject.layer) == "EnvironmentBlock")
		{
			currentStatus = Status.ToBeDestory;
			GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", Vector2.zero);
			DelayDestory(collisionDestroyDelayTime);
			return;
		}

		Creature receiver = null;
		if (TargetType == TargetType.Hero)
		{
			receiver = collision.GetComponent<Hero>();			
		}
		else if (TargetType == TargetType.Enemy)
		{
			receiver = collision.GetComponent<Enemy>();
		}
		if (receiver == null) return;
		CreateProjectileAttackRange();
		currentStatus = Status.ToBeDestory;
		GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", Vector2.zero);
		DelayDestory(collisionDestroyDelayTime);
	}

	protected virtual void CreateProjectileAttackRange()
	{
		throw new NotImplementedException();
	}

	private void DelayDestory(float delayTime)
	{
		delayDestorySequence?.Kill();

		delayDestorySequence = DOTween.Sequence();
		delayDestorySequence.AppendInterval(delayTime);
		delayDestorySequence.AppendCallback(() => { GameObject.Destroy(gameObject); });
	}

	private void OnHeroStrikeBack(Creature attacker)
	{
		if (Target)
		{
			Target = Owner.gameObject;
		}

		Direction = new Vector2(-Direction.x, -Direction.y);
		GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", Direction * MaxSpeed);
		TargetType = TargetType.Enemy;
		Owner = attacker;
		currentStatus = Status.Running;
		DelayDestory(maxExistTime);
		
		
		
	}
}
