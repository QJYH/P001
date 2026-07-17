using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveManager : MonoBehaviour
{
	private Dictionary<string, Velocity> velocityDict = new Dictionary<string, Velocity>();

	public UnityEvent<Vector2> OnSetVelocity;
	private void FixedUpdate()
	{
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
		if (rigidbody2D.bodyType == RigidbodyType2D.Static) return;

			Vector2 totalVelocity = Vector2.zero;
		foreach (var keyValuePair in velocityDict)
		{
			Velocity velocityInfo = keyValuePair.Value;
			Vector2 acceleration = velocityInfo.TargetVelocity - velocityInfo.CurrentVelocity;
			acceleration = Mathf.Min(velocityInfo.MaxAcceleration * Time.fixedDeltaTime, acceleration.magnitude) * acceleration.normalized;
			velocityInfo.CurrentVelocity += acceleration;
			totalVelocity += velocityInfo.CurrentVelocity;
		}

		rigidbody2D.velocity = totalVelocity;
		OnSetVelocity.Invoke(totalVelocity);
	}

	public void RemoveAllVelocity()
	{
		velocityDict.Clear();
	}

	public void AddVelocity(string velocityName, Velocity velocity)
	{
		velocityDict.Remove(velocityName);
		velocityDict.Add(velocityName, velocity);
	}

	public void RemoveVelocity(string velocityName)
	{
		velocityDict.Remove(velocityName);
	}

	public Velocity GetVelocity(string velocityName)
	{
		return velocityDict[velocityName];
	}

	public void ChangeCurrentVelocity(string velocityName, Vector2 currentVelocity)
	{
		if (!velocityDict.ContainsKey(velocityName)) return;

		Velocity velocity = velocityDict[velocityName];
		velocity.CurrentVelocity = currentVelocity;
	}

	public void ChangeTargetVelocity(string velocityName, Vector2 targetVelocity)
	{
		if (!velocityDict.ContainsKey(velocityName)) return;

		Velocity velocity = velocityDict[velocityName];
		velocity.TargetVelocity = targetVelocity;
	}

	public void ChangeMaxAcceleration(string velocityName, float maxAcceleration)
	{
		if (!velocityDict.ContainsKey(velocityName)) return;

		Velocity velocity = velocityDict[velocityName];
		velocity.MaxAcceleration = maxAcceleration;
	}
}
