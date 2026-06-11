using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttackRange : CreatureRange
{
	private CreatureAttack ownerAttack;

	public void SetOwnerAttack(CreatureAttack creatureAttack)
	{
		ownerAttack = creatureAttack;
	}
}
