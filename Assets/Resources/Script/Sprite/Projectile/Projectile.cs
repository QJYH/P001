using CommonEnum;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	protected virtual float MaxSpeed { get; } = 0;
	protected virtual float MaxAcceleration { get; } = 0;
	protected virtual float AngleSpeed { get; } = 0;
	protected virtual float MaxExistTime { get; } = 5;
	protected virtual float CollisionDestroyDelayTime { get; } = 0.3f;
	protected virtual bool CanBeStruckBack { get; } = true;
	protected virtual string AttackRangeDirName { get; } = "";
	protected virtual float AttackRangeDurationTime { get; } = 0.1f;


	public Creature Owner { get; set; }
	public GameObject Target { get; set; }
	public TargetType TargetType { get; set; }
	public HarmInfo HarmInfo { get; set; }
	public Vector2 Direction { get; set; }
	protected Tween DelayDestoryTween { get; set; }

	private void Start()
	{
		AddBaseMoveVelocity();
		DelayDestory(MaxExistTime);
	}

	private void OnDestroy()
	{
		DelayDestoryTween?.Kill();
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
			HitAttackRange(attackRange);
			return;
		}

		if (LayerMask.LayerToName(collision.gameObject.layer) == "EnvironmentBlock")
		{
			HitEnvironmentBlock();
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
		if (receiver)
		{
			HitTarget();
			return;
		}		
	}

	protected virtual void HitTarget()
	{
		CreateProjectileAttackRange();
		GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", Vector2.zero);
		DelayDestory(CollisionDestroyDelayTime);
		Target = null;
	}

	protected virtual void HitEnvironmentBlock()
	{
		GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", Vector2.zero);
		DelayDestory(CollisionDestroyDelayTime);
		Target = null;
	}

	private void HitAttackRange(AttackRange attackRange)
	{
		TryHeroStrikeBack(attackRange);
	}

	private void TryHeroStrikeBack(AttackRange attackRange)
	{
		if (attackRange.Attacker is not Hero) return;
		if (attackRange.TargetType != TargetType.Enemy) return;
		if (Owner is not Enemy) return;
		if (!CanBeStruckBack) return;

		Direction = new Vector2(-Direction.x, 0);
		GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", Direction * MaxSpeed);
		TargetType = TargetType.Enemy;
		Owner = attackRange.Attacker;
		DelayDestory(MaxExistTime);
		Target = null;
	}

	private void CreateProjectileAttackRange()
	{
		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(AttackRangeDirName, Owner, HarmInfo, TargetType, AttackRangeDurationTime);
		attackRangeObject.transform.position = transform.position;
	}

	protected void DelayDestory(float delayTime)
	{
		DelayDestoryTween?.Kill();

		DelayDestoryTween = DOVirtual.DelayedCall(delayTime, ()=> { Destroy(gameObject); });
	}	
}
