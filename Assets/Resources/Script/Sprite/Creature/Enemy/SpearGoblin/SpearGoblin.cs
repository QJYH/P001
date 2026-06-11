using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGoblin : Enemy
{
	private readonly float spearGoblinHp = 5f;
	protected override void InitHp()
	{
		hp = spearGoblinHp;
		maxHp = spearGoblinHp;
	}

	protected override void InitAttackList()
	{
		GameObject spearAttack = transform.Find("SpearAttack").gameObject;
		attackList.Add(spearAttack);
	}
}
