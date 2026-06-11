using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureRange : MonoBehaviour
{
	private Creature owner;

	public void SetOwner(Creature creature)
	{
		owner = creature;
	}
}
