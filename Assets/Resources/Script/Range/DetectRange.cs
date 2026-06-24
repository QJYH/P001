using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRange : Range
{
	private EnemyAttack<Enemy> enemyAttack;
	private void Awake()
	{
		enemyAttack = GetComponentInParent<EnemyAttack<Enemy>>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (enemyAttack == null) return;

		Hero hero = collision.GetComponent<Hero>();
		if (hero != null)
		{
			enemyAttack.OnTargetEnterDectectRange();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (enemyAttack == null) return;

		Hero hero = collision.GetComponent<Hero>();
		if (hero != null)
		{
			enemyAttack.OnTargetExitDectectRange();
		}
	}
}
