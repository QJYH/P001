using CommonEnum;
using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private const float destoryDelayTime = 0.1f;
	private enum Status
	{
		Running,
		ToBeDestory
	}
	
	public Vector2 Direction { get; set; }
	public GameObject Target { get; set; }
	public AttackTargetType TargetType { get; set; }
	public Creature Owner { get; set; }
	public HarmInfo HarmInfo { get; set; }
	protected float MaxSpeed { get; set; }
	private Status currentStatus;

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

		if (Target != null)
		{
			//todo
		}
		else
		{
			GetComponent<Rigidbody2D>().velocity = Direction * MaxSpeed;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (LayerMask.LayerToName(collision.gameObject.layer) == "EnvironmentBlock")
		{
			StartCoroutine(DelayDestory(destoryDelayTime));
			return;
		}

		Creature receiver = null;
		if (TargetType == AttackTargetType.Hero)
		{
			receiver = collision.GetComponent<Hero>();
			if (receiver == null) return;
		}
		else if (TargetType == AttackTargetType.Enemy)
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
