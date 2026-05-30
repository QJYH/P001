using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Warrior : Hero
{
	private readonly float warriorHp = 5f;
	private bool defend;
	private bool strikeBack;
	private bool defendAll;
	private bool defendHalf;

	protected override void InitHp()
	{
		hp = warriorHp;
	}

	protected override void InitAttackModule()
	{
		string lightAttackDir = string.Format("Prefab/Sprite/Creature/Hero/Warrior/LightAttack/{0}", "TripleSlash");
		string heavyAttackDir = string.Format("Prefab/Sprite/Creature/Hero/Warrior/HeavyAttack/{0}", "NormalGuard");
		lightAttack = Instantiate(Resources.Load<GameObject>(lightAttackDir), gameObject.transform);
		heavyAttack = Instantiate(Resources.Load<GameObject>(heavyAttackDir), gameObject.transform);
		GetComponent<Animator>().SetInteger("LightAttackCode", 101101);
		GetComponent<Animator>().SetInteger("HeavyAttackCode", 101201);

	}

	protected override void ReceiveHarm(float harmValue)
	{
		if (isStrikeBackEnable())
		{
			lightAttack.GetComponent<WarriorLightAttack>().DoFinisher();
		}
		else if (IsDefendAllEnable())
		{
			return;
		}
		else if (IsDefendHalfEnable())
		{
			base.ReceiveHarm(harmValue / 2);
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

	public void SetLightAttackNormalColliderEnable(int status)
	{
		WarriorLightAttackRange range = lightAttack.GetComponentInChildren<WarriorLightAttackRange>();
		range.GetComponent<PolygonCollider2D>().enabled = (status == 1) ? true : false;
	}

	public void SetLightAttackFinisherColliderEnable(int status)
	{
		WarriorLightAttackFinisherRange range = lightAttack.GetComponentInChildren<WarriorLightAttackFinisherRange>();
		range.GetComponent<PolygonCollider2D>().enabled = (status == 1) ? true : false;
	}

	public void SetDefendEnable(int status)
	{
		defend = (status == 1) ? true : false;
	}

	public void SetStrikeBackEnable(int status)
	{
		strikeBack = (status == 1) ? true : false;
	}

	public bool isStrikeBackEnable()
	{
		return strikeBack;
	}

	public void SetDefendAllEnable(int status)
	{
		defendAll = (status == 1) ? true : false;
	}

	public bool IsDefendAllEnable()
	{
		return defendAll;
	}

	public void SetDefendHalfEnable(int status)
	{
		defendHalf = (status == 1) ? true : false;
	}

	public bool IsDefendHalfEnable()
	{
		return defendHalf;
	}
}   
