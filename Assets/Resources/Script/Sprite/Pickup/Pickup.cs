using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	protected bool isPickedUp { get; private set; }
	protected virtual void OnPickUp(Hero hero)
	{
		throw new NotImplementedException();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hero hero = collision.GetComponent<Hero>();
		if (hero == null) return;

		GetComponent<BoxCollider2D>().enabled = false;
		isPickedUp = true;
		OnPickUp(hero);
	}
}
