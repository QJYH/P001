using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorLightAttackRange : CreatureRange
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Enemy enemy = collision.GetComponent<Enemy>();

		if (enemy != null)
		{
			Warrior warrior = GetComponentInParent<Warrior>();
			warrior.OnLightAttackHitEnemy(enemy, false);
		}
	}
}
