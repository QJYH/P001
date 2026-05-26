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

	public ChargeSlash(Warrior warrior)
	{
		this.warrior = warrior;
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
				DoFinisher(false);
			}
			else
			{
				warrior.GetComponent<Animator>().SetBool("IsLightAttackFinisher", false);
				warrior.GetComponent<Animator>().SetTrigger("LightAttack");
				chargeBeginTime = 0;
				isCharging = false;
			}
		}
	}

	public override void DoFinisher(bool withDefend)
	{
		warrior.GetComponent<Animator>().SetBool("IsLightAttackFinisher", true);
		warrior.GetComponent<Animator>().SetTrigger("LightAttack");
		chargeBeginTime = 0;
		isCharging = false;
		base.DoFinisher(withDefend);
	}

	
}
