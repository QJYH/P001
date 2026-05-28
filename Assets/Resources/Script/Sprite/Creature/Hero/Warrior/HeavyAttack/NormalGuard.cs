using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class NormalGuard : WarriorHeavyAttack
{
	public override void HandleHeavyAttackInput(InputAction.CallbackContext context)
	{
		if (warrior.IsDoingAction()) return;

		if (context.phase == InputActionPhase.Started)
		{		
			warrior.GetComponent<Animator>().SetTrigger("HeavyAttack");
		}
	}
	public override void OnAttackBegin()
	{
		warrior.SetDefendStatus(true);
		warrior.SetStrikeBackStatus(true);
		TimerManager.Instance.DelayCall(0.2f, () =>
		{
			if (warrior == null) return;
			warrior.SetStrikeBackStatus(false);
		});
	}

	public override void OnAttackEnd()
	{
		warrior.SetDefendStatus(false);
	}
}
