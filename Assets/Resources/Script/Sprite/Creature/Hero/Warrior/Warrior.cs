using UnityEngine;

public class Warrior : Hero
{
	public override float MaxHp { get; } = 1f;
	protected override float MaxRunSpeed { get; } = 5f;
	protected override float MaxRunAcceleration { get; } = 50f;

	public bool StrikeBackEnable { get; set; } = false;
	public bool DefendEnable { get; set; } = false;

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
		if (StrikeBackEnable)
		{
			HeavyAttack.GetComponent<WarriorHeavyAttack>().StrikeBack(LightAttack.GetComponent<WarriorLightAttack>());
		}
		else if (DefendEnable)
		{
			
		}
		else
		{
			base.ReceiveHarm(harmValue);
		}
	}

	public void SetStrikeBackEnable(int status)
	{
		//动画事件调用
		StrikeBackEnable = (status == 1) ? true : false;
	}
}   
