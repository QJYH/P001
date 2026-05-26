using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGoblin : Enemy
{
	private readonly float spearGoblinHp = 5f;
	private readonly float spearGoblinAttackHarm = 1f;
	protected override void InitHp()
	{
		hp = spearGoblinHp;
	}

	protected override void InitAttackHarm()
	{
		attackHarmValue = spearGoblinAttackHarm;
	}
}
