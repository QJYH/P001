using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectArea : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hero hero = collision.GetComponent<Hero>();
		if (hero != null)
		{
			GetComponentInParent<Enemy>().SetDetectAreaActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Hero hero = collision.GetComponent<Hero>();
		if (hero != null)
		{
			GetComponentInParent<Enemy>().SetDetectAreaActive(false);
		}
	}
}
