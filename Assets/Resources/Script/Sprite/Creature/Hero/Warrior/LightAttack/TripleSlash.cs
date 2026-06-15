using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class TripleSlash : WarriorLightAttack
{
	private readonly float tripleSlashNormalHarmValue = 1f;
	private readonly float tripleSlashFinisherHarmValue = 2f;
	private readonly float comboLimitTime = 1f;
	private int slashCount = 0;
	private float lastSlashTime = 0;

	protected override void Awake()
	{
		base.Awake();
		normalHarmValue = tripleSlashNormalHarmValue;
		finisherHarmValue = tripleSlashFinisherHarmValue;
	}

	public override void HandleLightAttackInput(InputAction.CallbackContext context)
	{
		if (warrior.IsDoingAction()) return;

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

	protected override string GetFinisherAttackRangeObjectDirName()
	{
		return "Prefab/Sprite/Creature/Hero/Warrior/LightAttack/TripleSlash/FinisherAttackRange";
	}

	protected override string GetNormalAttackRangeObjectDirName()
	{
		return "Prefab/Sprite/Creature/Hero/Warrior/LightAttack/TripleSlash/NormalAttackRange";
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
