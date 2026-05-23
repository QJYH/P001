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
	private Warrior warrior;

	public NormalGuard(Warrior warrior)
	{
		this.warrior = warrior;
	}

	public override void HandleHeavyAttackInput(InputAction.CallbackContext context)
	{
		if (warrior.IsDoingAction()) return;

		if (context.phase == InputActionPhase.Started)
		{		
			warrior.GetComponent<Animator>().SetTrigger("HeavyAttack");
		}
	}
}
