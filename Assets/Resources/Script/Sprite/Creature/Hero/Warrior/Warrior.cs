using UnityEngine;

public class Warrior : Hero
{
	protected override float MaxHp { get; } = 5f;

	private bool strikeBack;
	private bool defendAll;
	private bool defendHalf;

	protected override void InitAttackModule()
	{
		string lightAttackDir = string.Format("Prefab/Sprite/Creature/Hero/Warrior/LightAttack/{0}/{0}", "TripleSlash");
		string heavyAttackDir = string.Format("Prefab/Sprite/Creature/Hero/Warrior/HeavyAttack/{0}/{0}", "NormalGuard");
		LightAttack = Instantiate(Resources.Load<GameObject>(lightAttackDir), gameObject.transform);
		HeavyAttack = Instantiate(Resources.Load<GameObject>(heavyAttackDir), gameObject.transform);
		GetComponent<Animator>().SetInteger("LightAttackCode", 101101);
		GetComponent<Animator>().SetInteger("HeavyAttackCode", 101201);

	}

	protected override void ReceiveHarm(float harmValue)
	{
		if (IsStrikeBackEnable())
		{
			HeavyAttack.GetComponent<WarriorHeavyAttack<Warrior>>().StrikeBack(LightAttack.GetComponent<WarriorLightAttack<Warrior>>());
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

	public void SetStrikeBackEnable(int status)
	{
		strikeBack = (status == 1) ? true : false;
	}

	public bool IsStrikeBackEnable()
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
