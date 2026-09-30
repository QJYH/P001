using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Archer : Enemy
{
	public override float MaxHp { get; } = 4f;
	protected override float MaxRunSpeed { get; } = 3f;
	protected override float MaxRunAcceleration { get; } = 30f;

	protected override void InitAttackList()
	{
		GameObject shootArrow = transform.Find("ShootArrow").gameObject;
		AttackList.Add(shootArrow);
	}

	protected override void AdjustHpBarPos(RectTransform rectTransform)
	{
		rectTransform.anchoredPosition = new Vector2(0, -1f);
	}
}
