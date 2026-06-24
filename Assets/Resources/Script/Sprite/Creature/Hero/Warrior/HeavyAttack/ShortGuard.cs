using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShortGuard : WarriorHeavyAttack<Warrior>
{
	public override void HandleHeavyAttackInput(InputAction.CallbackContext context)
	{
		if (Owner.IsDoingAction()) return;

		if (context.phase == InputActionPhase.Started)
		{		
			Owner.GetComponent<Animator>().SetTrigger("HeavyAttack");
		}
	}

	public override void StrikeBack(WarriorLightAttack<Warrior> lightAttack)
	{
		lightAttack.DoFinisher(2);
	}
}
