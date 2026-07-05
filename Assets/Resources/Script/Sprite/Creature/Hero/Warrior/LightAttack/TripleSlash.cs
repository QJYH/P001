using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class TripleSlash : WarriorLightAttack
{
	private const float comboLimitTime = 1f;
	protected override float NormalHarmValue { get; } = 1f;
	protected override float FinisherHarmValue { get; } = 2f;
	protected override string NormalAttackRangeObjectDirName { get; } = "Prefab/Sprite/Creature/Hero/Warrior/LightAttack/TripleSlash/NormalAttackRange";
	protected override string FinisherAttackRangeObjectDirName { get; } = "Prefab/Sprite/Creature/Hero/Warrior/LightAttack/TripleSlash/FinisherAttackRange";
	protected override float NormalAttackRangeDurationTime { get; } = 0.1f;
	protected override float FinisherAttackRangeDurationTime { get; } = 0.1f;

	private int slashCount = 0;
	private float lastSlashTime = 0;

	public override void HandleLightAttackInput(InputAction.CallbackContext context)
	{
		if (((Hero)Owner).IsDoingAction()) return;

		if (context.phase == InputActionPhase.Canceled)
		{
			float currentTime = Time.time;
			if ((currentTime - lastSlashTime) > comboLimitTime) slashCount = 0;
			lastSlashTime = currentTime;
			slashCount += 1;

			if (slashCount == 3)
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
		base.DoNormal();
	}

	public override void DoFinisher(float harmRate)
	{
		lastSlashTime = 0;
		slashCount = 0;
		base.DoFinisher(harmRate);
	}
}
