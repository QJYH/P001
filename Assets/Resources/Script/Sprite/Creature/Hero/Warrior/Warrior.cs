using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Warrior : Hero
{
	private readonly float warriorHp = 5f;
	private bool isDefending;
	private bool canStrikeBack;
	

	protected override void InitHp()
	{
		hp = warriorHp;
	}

	protected override void InitAttackModule()
	{
		lightAttack = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Creature/Hero/Warrior/LightAttack/TripleSlash"));
		heavyAttack = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Creature/Hero/Warrior/HeavyAttack/NormalGuard"));
		lightAttack.transform.SetParent(gameObject.transform, false);
		heavyAttack.transform.SetParent(gameObject.transform, false);
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
			lightAttack.GetComponent<WarriorLightAttack>().DoFinisher(true);
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

	public void OnLightAttackHitEnemy(Enemy enemy, bool isFinisher)
	{
		float harmValue = lightAttack.GetComponent<WarriorLightAttack>().GetHarmValue(isFinisher);
		Creature.HandleAttack(this, enemy, harmValue);
	}

	public void EnableLightAttackCollider(int status)
	{
		WarriorLightAttackRange range = lightAttack.GetComponentInChildren<WarriorLightAttackRange>();
		range.GetComponent<PolygonCollider2D>().enabled = (status == 1) ? true : false;
	}

	public void EnableLightAttackFinisherCollider(int status)
	{
		WarriorLightAttackFinisherRange range = lightAttack.GetComponentInChildren<WarriorLightAttackFinisherRange>();
		range.GetComponent<PolygonCollider2D>().enabled = (status == 1) ? true : false;
	}
}   
