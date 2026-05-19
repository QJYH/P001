using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	private GameObject target;
	private TargetType targetType;
	public enum TargetType
	{
		Hero,
		Area
	}
  
    void FixedUpdate()
    {
		FollowTarget();
    }

	private void FollowTarget()
	{
		if (target == null) return;

		switch (targetType)
		{
			case TargetType.Hero:
				FollowHero();
				break;
			case TargetType.Area:
				FollowArea();
				break;
		}
	

	}

	private void FollowArea()
	{
		
	}

	private void FollowHero()
	{
		Vector3 targetPos = target.transform.position;
		float heroHeight = 3;
		gameObject.transform.position = new Vector3(targetPos.x, targetPos.y + heroHeight/2, gameObject.transform.position.z);
	}

	public void SetTarget(GameObject obj, TargetType tarType)
	{
		target = obj;
		targetType = tarType;
		FollowTarget();
	}
}
