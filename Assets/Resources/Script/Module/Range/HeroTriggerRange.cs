using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroTriggerRange : Range
{
	public UnityEvent OnHeroEnter = new UnityEvent();
	public UnityEvent OnHeroExit = new UnityEvent();

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Hero>())
		{
			OnHeroEnter.Invoke();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<Hero>())
		{
			OnHeroExit.Invoke();
		}
	}
}
