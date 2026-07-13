using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : Particle
{
	public void OnHealAnimationBegin()
	{
		GetComponentInParent<Hero>().OnHealAnimationBegin();
	}

	public void OnHealAnimationEnd()
	{
		GetComponentInParent<Hero>().OnHealAnimationEnd();
		Destroy(gameObject);
	}
}
