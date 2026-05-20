using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Hero Target { get; set; }

	public void GoToPos(Vector2 value)
	{
		throw new NotImplementedException();
	}

	internal bool IsTargetInAttackArea()
	{
		throw new NotImplementedException();
	}

	internal void TryAttack()
	{
		throw new NotImplementedException();
	}
}
