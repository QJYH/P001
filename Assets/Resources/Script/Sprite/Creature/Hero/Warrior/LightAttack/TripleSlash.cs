using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class TripleSlash : WarriorLightAttack
{
	
	private readonly float comboLimitTime = 1f;
	private int slashCount = 0;
	private float lastSlashTime = 0;

	protected override void Awake()
	{
		base.Awake();
		normalHarmValue = 1f;
		finisherHarmValue = 2f;
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
}
