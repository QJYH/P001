using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Warrior : Hero
{
	private WarriorLightAttack lightAttack;
	private WarriorHeavyAttack heavyAttack;

	protected override void InitAttackModule()
	{
		lightAttack = (WarriorLightAttack)HeroAttack.CreateHeroAttack(nameof(TripleSlash), this);
		heavyAttack = (WarriorHeavyAttack)HeroAttack.CreateHeroAttack(nameof(NormalGuard), this);
		
	}

	protected override void HandleLightAttackInput(InputAction.CallbackContext context)
	{
		lightAttack.HandleLightAttackInput(context);
	}

	protected override void HandleHeavyAttackInput(InputAction.CallbackContext context)
	{
		heavyAttack.HandleHeavyAttackInput(context);
	}

	private void OnDestroy()
	{
		Destroy(this.lightAttack);
		Destroy(this.heavyAttack);
	}
}   
