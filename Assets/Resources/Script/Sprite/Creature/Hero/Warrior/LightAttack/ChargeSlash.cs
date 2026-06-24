using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeSlash : WarriorLightAttack<Warrior>
{
	private const float chargeNeedTime = 1f;

	protected override float NormalHarmValue { get; } = 1f;
	protected override float FinisherHarmValue { get; } = 2f;
	protected override string NormalAttackRangeObjectDirName { get; } = "Prefab/Sprite/Creature/Hero/Warrior/LightAttack/ChargeSlash/NormalAttackRange";
	protected override string FinisherAttackRangeObjectDirName { get; } = "Prefab/Sprite/Creature/Hero/Warrior/LightAttack/ChargeSlash/FinisherAttackRange";
	protected override float NormalAttackRangeDurationTime { get; } = 0.1f;
	protected override float FinisherAttackRangeDurationTime { get; } = 0.1f;

	private float chargeBeginTime = 0;
	private bool isCharging = false;

	public override void HandleLightAttackInput(InputAction.CallbackContext context)
	{
		if (Owner.IsDoingAction()) return;

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
}
