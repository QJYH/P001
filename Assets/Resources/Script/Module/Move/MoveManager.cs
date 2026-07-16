using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
	private Dictionary<string, Velocity> velocityDict = new Dictionary<string, Velocity>();
	private void FixedUpdate()
	{
		Vector2 totalVelocity = Vector2.zero;
		foreach (var keyValuePair in velocityDict)
		{
			Velocity velocityInfo = keyValuePair.Value;
			Vector2 acceleration = velocityInfo.TargetVelocity - velocityInfo.CurrentVelocity;
			acceleration = Mathf.Min(velocityInfo.MaxAcceleration * Time.fixedDeltaTime, acceleration.magnitude) * acceleration.normalized;
			velocityInfo.CurrentVelocity += acceleration;
			totalVelocity += velocityInfo.CurrentVelocity;
		}

		GetComponent<Rigidbody2D>().velocity = totalVelocity;
	}

	public void AddVelocity(string name, Velocity velocityInfo)
	{
		velocityDict.Remove(name);
		velocityDict.Add(name, velocityInfo);
	}

	public void RemoveVelocity(string name)
	{
		velocityDict.Remove(name);
	}

	public Velocity GetVelocity(string name)
	{
		return velocityDict[name];
	}

	public void ChangeVelocity(string name, Vector2? currentVelocity, Vector2? targetVelocity, float? maxAcceleration)
	{
		if (!velocityDict.ContainsKey(name)) return;

		Velocity velocityInfo = velocityDict[name];
		velocityInfo.CurrentVelocity = currentVelocity ?? velocityInfo.CurrentVelocity;
		velocityInfo.TargetVelocity = targetVelocity ?? velocityInfo.TargetVelocity;
		velocityInfo.MaxAcceleration = maxAcceleration ?? velocityInfo.MaxAcceleration;
	}
}
