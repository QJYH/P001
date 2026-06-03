using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
	protected float hp;
	protected bool isDead = false;
	private readonly float hurtDuration = 0.4f;
	private float hurtTimer = 0;

	protected virtual void Awake()
	{
		InitHp();
	}

	protected virtual void Update()
	{
		UpdateHurtTimer();
	}

	private void UpdateHurtTimer()
	{
		if (hurtTimer > 0)
		{
			hurtTimer -= Time.deltaTime;
		}
		float hurtColorRate = (hurtTimer > hurtDuration / 2) ? ((hurtDuration - hurtTimer) * 4) : (hurtTimer * 4);
		GetComponent<SpriteRenderer>().material.SetFloat("_HurtColorRate", hurtTimer > 0 ? hurtColorRate : 0);

	}

	protected virtual void InitHp()
	{
		throw new NotImplementedException();
	}

	public static void HandleAttack(Creature attacker, Creature receiver, HarmInfo harmInfo)
	{
		if (receiver.IsDead()) return;

		float finalHarm = harmInfo.CalFinalHarm();
		receiver.ReceiveHarm(finalHarm);
	}

	protected virtual void ReceiveHarm(float harmValue)
	{
		if (harmValue <= 0) return;

		hp -= harmValue;
		hurtTimer = hurtDuration;
		if (hp <= 0)
		{
			Die();
		}
	}

	protected void Die()
	{
		enabled = false;
		isDead = true;
		hurtTimer = 0;
		GetComponent<Animator>().SetTrigger("Die");
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
	}

	public bool IsDead()
	{
		return isDead;
	}
}
