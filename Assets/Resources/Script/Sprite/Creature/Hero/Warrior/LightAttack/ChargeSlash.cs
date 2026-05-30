using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeSlash : WarriorLightAttack
{
	private readonly float chargeNeedTime = 1f;
	private float chargeBeginTime = 0;
	private bool isCharging = false;

	protected override void Awake()
	{
		base.Awake();
		normalHarmValue = 1f;
		finisherHarmValue = 2f;
	}

	public override void HandleLightAttackInput(InputAction.CallbackContext context)
	{
		if (warrior.IsDoingAction()) return;

		if (context.phase == InputActionPhase.Started)
		{
			chargeBeginTime = Time.time;
			isCharging = true;
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			float chargeDuration = Time.time - chargeBeginTime;
			if (isCharging && (chargeDuration > chargeNeedTime))
			{
				DoFinisher();
			}
			else
			{
				DoNormal();
			}
		}
	}

	private void DoNormal()
	{
		currentPhase = LightAttackPhase.Normal;
		warrior.GetComponent<Animator>().SetInteger("LightAttackPhase", 1);
		warrior.GetComponent<Animator>().SetTrigger("LightAttack");
		chargeBeginTime = 0;
		isCharging = false;
	}

	public override void DoFinisher()
	{
		currentPhase = LightAttackPhase.Finisher;
		warrior.GetComponent<Animator>().SetInteger("LightAttackPhase", 2);
		warrior.GetComponent<Animator>().SetTrigger("LightAttack");
		chargeBeginTime = 0;
		isCharging = false;
	}

	
}
