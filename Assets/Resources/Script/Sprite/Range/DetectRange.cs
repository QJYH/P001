using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRange : Range
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hero hero = collision.GetComponent<Hero>();
		if (hero != null)
		{
			GetComponentInParent<EnemyAttack>().OnTargetEnterDectectRange();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Hero hero = collision.GetComponent<Hero>();
		if (hero != null)
		{
			GetComponentInParent<EnemyAttack>().OnTargetExitDectectRange();
		}
	}
}
