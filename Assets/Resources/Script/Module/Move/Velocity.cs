using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity
{
	public Vector2 CurrentVelocity { get; set; } = Vector2.zero;
	public Vector2 TargetVelocity { get; set; } = Vector2.zero;
	public float MaxAcceleration { get; set; } = 0;

	public Velocity(Vector2 currentVelocity, Vector2 targetVelocity, float maxAcceleration)
	{
		CurrentVelocity = currentVelocity;
		TargetVelocity = targetVelocity;
		MaxAcceleration = maxAcceleration;
	}

	public Velocity()
	{

	}
}
