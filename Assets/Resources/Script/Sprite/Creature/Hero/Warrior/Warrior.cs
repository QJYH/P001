using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Warrior : Hero
{
	private readonly float warriorHp = 5f;
	private WarriorLightAttack lightAttack;
	private WarriorHeavyAttack heavyAttack;
	private bool isDefending;
	private bool canStrikeBack;
	

	protected override void InitHp()
	{
		hp = warriorHp;
	}

	protected override void InitAttackModule()
	{
		lightAttack = WarriorLightAttack.Create(nameof(TripleSlash), this);
		heavyAttack = WarriorHeavyAttack.Create(nameof(NormalGuard), this);
		
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

	public override void OnHeavyAttackBegin()
	{
		heavyAttack.OnAttackBegin();
		base.OnHeavyAttackBegin();
	}

	public override void OnHeavyAttackEnd()
	{
		heavyAttack.OnAttackEnd();
		base.OnHeavyAttackEnd();
	}

	public override void OnLightAttackBegin()
	{
		lightAttack.OnAttackBegin();
		base.OnLightAttackBegin();
	}

	public override void OnLightAttackEnd()
	{
		lightAttack.OnAttackEnd();
		base.OnLightAttackEnd();
	}

	public void SetDefendStatus(bool value)
	{
		isDefending = value;
	}

	public void SetStrikeBackStatus(bool value)
	{
		canStrikeBack = value;
	}

	protected override void ReceiveHarm(float harmValue)
	{
		if (canStrikeBack)
		{
			SetStrikeBackStatus(false);
			lightAttack.DoFinisher(true);
		}
		else if (isDefending)
		{
			return;
		}
		else
		{
			base.ReceiveHarm(harmValue);
		}
	}

	
}   
