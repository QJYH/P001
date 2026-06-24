using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGoblin : Enemy
{
	protected override float MaxHp { get; } = 5f;

	protected override void InitAttackList()
	{
		GameObject spearAttack = transform.Find("SpearAttack").gameObject;
		AttackList.Add(spearAttack);
	}
}
