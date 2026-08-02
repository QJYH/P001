using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFunc : MonoBehaviour
{
	public static bool IsNear(float numberA, float numberB, float? threshold)
	{
		threshold = threshold ?? 0.1f;
		return Mathf.Abs(numberA - numberB) < threshold;
	}
    public static bool IsNear(Vector2 beginWorldPos, Vector2 endWorldPos, float? threshold)
	{
		threshold = threshold ?? 0.1f;
		return Vector2.Distance(beginWorldPos, endWorldPos) < threshold;
	}
}
