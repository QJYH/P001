using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
	protected float hp;
	protected bool isDead = false;

	protected virtual void Awake()
	{
		InitHp();
	}

	protected virtual void InitHp()
	{

	}

	public static void HandleAttack(Creature attacker, Creature receiver, float harmValue)
	{
		if (receiver.IsDead()) return;

		receiver.ReceiveHarm(harmValue);
	}

	protected virtual void ReceiveHarm(float harmValue)
	{
		hp -= harmValue;
		if (hp <= 0)
		{
			Die();
		}
	}

	protected void Die()
	{
		enabled = false;
		isDead = true;
		GetComponent<Animator>().SetTrigger("Die");
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
	}

	public bool IsDead()
	{
		return isDead;
	}
}
