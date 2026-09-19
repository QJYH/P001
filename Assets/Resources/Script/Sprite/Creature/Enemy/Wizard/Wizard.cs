using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
	public override float MaxHp { get; } = 5f;
	protected override float MaxRunSpeed { get; } = 3f;
	protected override float MaxRunAcceleration { get; } = 30f;

	protected override void InitAttackList()
	{
		GameObject shootMagicOrb = transform.Find("ShootMagicOrb").gameObject;
		AttackList.Add(shootMagicOrb);
	}
	protected override void AdjustHpBarPos(RectTransform rectTransform)
	{
		rectTransform.anchoredPosition = new Vector2(0, -1f);
	}
}
