using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hero hero = collision.GetComponent<Hero>();
		
		if (hero != null)
		{
			Enemy enemy = GetComponentInParent<Enemy>();
			Creature.HandleAttack(enemy, hero, enemy.GetAttackHarmValue());
		}
	}
}
