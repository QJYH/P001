using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeSlash : WarriorLightAttack
{
	private readonly float chargeSlashNormalHarmValue = 1f;
	private readonly float chargeSlashFinisherHarmValue = 2f;
	private readonly float chargeNeedTime = 1f;
	private float chargeBeginTime = 0;
	private bool isCharging = false;

	protected override void Awake()
	{
		base.Awake();
		normalHarmValue = chargeSlashNormalHarmValue;
		finisherHarmValue = chargeSlashFinisherHarmValue;
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
				DoFinisher(1);
			}
			else
			{
				DoNormal();
			}
		}
	}

	public override void DoNormal()
	{
		chargeBeginTime = 0;
		isCharging = false;
		base.DoNormal();
	}

	public override void DoFinisher(float harmRate)
	{
		chargeBeginTime = 0;
		isCharging = false;
		base.DoFinisher(harmRate);
	}

	protected override string GetFinisherAttackRangeObjectDirName()
	{
		return "Prefab/Sprite/Creature/Hero/Warrior/LightAttack/ChargeSlash/FinisherAttackRange";
	}

	protected override string GetNormalAttackRangeObjectDirName()
	{
		return "Prefab/Sprite/Creature/Hero/Warrior/LightAttack/ChargeSlash/NormalAttackRange";
	}

	protected override float GetFinisherAttackRangeDurationTime()
	{
		return 0.1f;
	}

	protected override float GetNormalAttackRangeDurationTime()
	{
		return 0.1f;
	}
}
