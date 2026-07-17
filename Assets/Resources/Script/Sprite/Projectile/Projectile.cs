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
	public TargetType TargetType { get; set; }
	public Creature Owner { get; set; }
	public HarmInfo HarmInfo { get; set; }
	protected virtual float MaxSpeed { get; set; } = 0;
	protected virtual float MaxAcceleration { get; set; } = 0;
	protected virtual float MaxExistTime { get; set; } = 5f;
	private Status currentStatus;

	private void Start()
	{
		currentStatus = Status.Running;
		AddBaseMoveVelocity();
		StartCoroutine(DelayDestory(MaxExistTime));
	}

	private void AddBaseMoveVelocity()
	{
		Vector2 targetVelocity = Direction * MaxSpeed;
		Velocity velocity = new Velocity(Vector2.zero, targetVelocity, MaxAcceleration);
		GetComponent<MoveManager>().AddVelocity("_BaseMove", velocity);
	}

	private void FixedUpdate()
	{
		TryFollowTarget();
	}

	private void TryFollowTarget()
	{
		if (currentStatus == Status.ToBeDestory) return;
		if (Target == null) return;

		//todo
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (LayerMask.LayerToName(collision.gameObject.layer) == "EnvironmentBlock")
		{
			currentStatus = Status.ToBeDestory;
			GetComponent<MoveManager>().RemoveAllVelocity();
			StartCoroutine(DelayDestory(destoryDelayTime));
			return;
		}

		Creature receiver = null;
		if (TargetType == TargetType.Hero)
		{
			receiver = collision.GetComponent<Hero>();
			if (receiver == null) return;
		}
		else if (TargetType == TargetType.Enemy)
		{
			receiver = collision.GetComponent<Enemy>();
			if (receiver == null) return;
		}
		CreateProjectileAttackRange();
		currentStatus = Status.ToBeDestory;
		GetComponent<MoveManager>().RemoveAllVelocity();
		StartCoroutine(DelayDestory(destoryDelayTime));
	}

	protected virtual void CreateProjectileAttackRange()
	{
		throw new NotImplementedException();
	}

	private IEnumerator DelayDestory(float durationTime)
	{
		yield return new WaitForSeconds(durationTime);

		GameObject.Destroy(gameObject);
	}
}
