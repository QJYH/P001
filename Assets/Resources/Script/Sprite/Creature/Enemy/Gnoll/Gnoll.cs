using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnoll : Enemy
{
	private readonly float gnollHp = 5f;
	protected override void InitHp()
	{
		Hp = gnollHp;
		MaxHp = gnollHp;
	}

	protected override void InitAttackList()
	{
		GameObject spearAttack = transform.Find("ThrowBone").gameObject;
		attackList.Add(spearAttack);
	}
	protected override void AdjustHpBarPos(RectTransform rectTransform)
	{
		rectTransform.anchoredPosition = new Vector2(0, -0.3f);
	}
}
