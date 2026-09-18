using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnoll : Enemy
{
	public override float MaxHp { get; } = 5f;
	protected override float MaxRunSpeed { get; } = 3f;
	protected override float MaxRunAcceleration { get; } = 30f;

	protected override void InitAttackList()
	{
		GameObject throwBone = transform.Find("ThrowBone").gameObject;
		AttackList.Add(throwBone);
	}
	protected override void AdjustHpBarPos(RectTransform rectTransform)
	{
		rectTransform.anchoredPosition = new Vector2(0, -1f);
	}
}
