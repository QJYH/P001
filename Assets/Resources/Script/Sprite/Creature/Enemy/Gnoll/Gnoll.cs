using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnoll : Enemy
{
	protected override float MaxHp { get; } = 5f;

	protected override void InitAttackList()
	{
		GameObject spearAttack = transform.Find("ThrowBone").gameObject;
		AttackList.Add(spearAttack);
	}
	protected override void AdjustHpBarPos(RectTransform rectTransform)
	{
		rectTransform.anchoredPosition = new Vector2(0, -0.3f);
	}
}
