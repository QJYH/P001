using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnoll : Enemy
{
	private readonly float gnollHp = 5f;
	protected override void InitHp()
	{
		hp = gnollHp;
		maxHp = gnollHp;
	}

}
