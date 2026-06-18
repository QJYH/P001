using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using static AttackRange;

public class Projectile : MonoBehaviour
{
	private enum Status
	{
		Running,
		ToBeDestory
	}
	private Status currentStatus;
	private readonly float destoryDelayTime = 0.1f;
	private Vector2 direction;
	protected float maxSpeed;
	private GameObject target;
	private TargetType targetType;
	protected Creature owner;
	protected HarmInfo harmInfo;

	private void Awake()
	{
		currentStatus = Status.Running;
		InitMaxSpeed();
	}

	protected virtual void InitMaxSpeed()
	{
		throw new NotImplementedException();
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		if (currentStatus == Status.ToBeDestory) return;

		if (target != null)
		{
			//todo
		}
		else
		{
			GetComponent<Rigidbody2D>().velocity = direction * maxSpeed;
		}
	}

	public void SetDirection(Vector2 direction)
	{
		this.direction = direction.normalized;
	}

	public void SetTargetType(TargetType targetType)
	{
		this.targetType = targetType;
	}

	public void SetOwner(Creature owner)
	{
		this.owner = owner;
	}

	public void SetHarmInfo(HarmInfo harmInfo)
	{
		this.harmInfo = harmInfo;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (LayerMask.LayerToName(collision.gameObject.layer) == "Decoration")
		{
			StartCoroutine(DelayDestory(destoryDelayTime));
			return;
		}

		Creature receiver = null;
		if (targetType == TargetType.Hero)
		{
			receiver = collision.GetComponent<Hero>();
			if (receiver == null) return;
		}
		else if (targetType == TargetType.Enemy)
		{
			receiver = collision.GetComponent<Enemy>();
			if (receiver == null) return;
		}
		CreateProjectileAttackRange();
		StartCoroutine(DelayDestory(destoryDelayTime));
	}

	protected virtual void CreateProjectileAttackRange()
	{
		throw new NotImplementedException();
	}

	private IEnumerator DelayDestory(float durationTime)
	{
		currentStatus = Status.ToBeDestory;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		yield return new WaitForSeconds(durationTime);

		GameObject.Destroy(gameObject);
	}
}
