using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Gold : Pickup
{
	private const float maxSpeed = 20f;
	private const float maxAcceleration = 100f;
	private const float destoryDistance = 0.5f;

	[SerializeField] private int goldValue;
	private Vector2 goldIconScreenPos;

	protected override void OnPickUp()
	{
		goldIconScreenPos = PanelLevelHUD.GetGoldIconScreenPos();
		GetComponent<SpriteRenderer>().sortingLayerName = "UI";

		Vector3 targetWorldPos = CalTargetWorldPos();
		Vector2 targetVelocity = maxSpeed * ((targetWorldPos - transform.position).normalized);
		Velocity velocity = new Velocity(Vector2.zero, targetVelocity, maxAcceleration);
		GetComponent<MoveManager>().AddVelocity("_GoToGoldIcon", velocity);
	}

	private void FixedUpdate()
	{
		if (!isPickedUp) return;

		Vector3 targetWorldPos = CalTargetWorldPos();
		if (Vector3.Distance(targetWorldPos, transform.position) < destoryDistance)
		{
			Destroy(gameObject);
			PanelLevelHUD.OnPickUpGold(goldValue);
		}
		else
		{
			Vector2 targetVelocity = maxSpeed * ((targetWorldPos - transform.position).normalized);
			GetComponent<MoveManager>().ChangeTargetVelocity("_GoToGoldIcon", targetVelocity);
		}
	}

	private Vector3 CalTargetWorldPos()
	{
		Vector3 targetScreenPos = new Vector3(goldIconScreenPos.x, goldIconScreenPos.y, 0);
		Vector3 targetWorldPos = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>().ScreenToWorldPoint(targetScreenPos);
		targetWorldPos.z = 0;

		return targetWorldPos;
	}
}
