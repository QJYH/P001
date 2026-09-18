using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGoblin : Enemy
{
	public override float MaxHp { get; } = 5f;
	protected override float MaxRunSpeed { get; } = 4f;
	protected override float MaxRunAcceleration { get; } = 40f;

	protected override void InitAttackList()
	{
		GameObject spearAttack = transform.Find("SpearAttack").gameObject;
		AttackList.Add(spearAttack);
	}

	protected override void AdjustHpBarPos(RectTransform rectTransform)
	{
		rectTransform.anchoredPosition = new Vector2(0, -1f);
	}

}
