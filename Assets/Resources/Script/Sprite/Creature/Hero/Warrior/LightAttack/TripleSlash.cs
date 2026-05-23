using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class TripleSlash : WarriorLightAttack
{
	private Warrior warrior;
	private readonly float comboLimitTime = 1f;
	private int slashCount = 0;
	private float lastSlashTime = 0;

	public TripleSlash(Warrior warrior)
	{
		this.warrior = warrior;
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
				DoFinisher();
			}
			else
			{
				warrior.GetComponent<Animator>().SetBool("IsLightAttackFinisher", false);
				warrior.GetComponent<Animator>().SetTrigger("LightAttack");
			}
		}
	}

	public override void DoFinisher()
	{
		warrior.GetComponent<Animator>().SetBool("IsLightAttackFinisher", true);
		warrior.GetComponent<Animator>().SetTrigger("LightAttack");
		lastSlashTime = 0;
		slashCount = 0;
	}
}
