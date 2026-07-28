using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRange : Range
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hero hero = collision.GetComponent<Hero>();
		if (hero != null)
		{
			GetComponentInParent<Enemy>().SeeHero(hero);
		}
	}
}
