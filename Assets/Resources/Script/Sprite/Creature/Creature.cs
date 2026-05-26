using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
	protected float hp;

	protected virtual void Awake()
	{
		InitHp();
	}

	protected virtual void InitHp()
	{

	}

	public static void HandleAttack(Creature attacker, Creature receiver, float harmValue)
	{
		receiver.ReceiveHarm(harmValue);
	}

	protected virtual void ReceiveHarm(float harmValue)
	{
		hp -= harmValue;
		if (hp <= 0) Die();
	}

	protected void Die()
	{
		GetComponent<Animator>().SetTrigger("Die");
	}
}
