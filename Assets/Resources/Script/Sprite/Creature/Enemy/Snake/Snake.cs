using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{
	public override float MaxHp { get; } = 5f;
	protected override float MaxRunSpeed { get; } = 6f;
	protected override float MaxRunAcceleration { get; } = 60f;

	protected override void InitAttackList()
	{
		GameObject snakeAttack = transform.Find("SnakeAttack").gameObject;
		AttackList.Add(snakeAttack);
	}

	protected override void AdjustHpBarPos(RectTransform rectTransform)
	{
		rectTransform.anchoredPosition = new Vector2(0, -1f);
	}
}
