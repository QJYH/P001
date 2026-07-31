using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFunc : MonoBehaviour
{
    public static bool IsNear(Vector2 beginWorldPos, Vector2 endWorldPos)
	{
		float distance = Vector2.Distance(beginWorldPos, endWorldPos);
		return (distance < 0.1f);
	}
}
