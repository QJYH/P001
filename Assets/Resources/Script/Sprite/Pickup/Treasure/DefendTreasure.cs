using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendTreasure : Pickup
{
	private const float maxSpeed = 20f;
	private const float maxAcceleration = 100f;
	private const float destoryDistance = 0.5f;
	private const int defendValue = 1;

	private Vector2 defendIconScreenPos;

	protected override void OnPickUp(Hero hero)
	{
		hero.PickUpDefendTreasure(defendValue);

		defendIconScreenPos = PanelLevelHUD.GetDefendIconScreenPos();
		GetComponent<SpriteRenderer>().sortingLayerName = "UI";

		Vector3 targetWorldPos = CalTargetWorldPos();
		Vector2 targetVelocity = maxSpeed * ((targetWorldPos - transform.position).normalized);
		Velocity velocity = new Velocity(Vector2.zero, targetVelocity, maxAcceleration);
		GetComponent<MoveManager>().AddVelocity("_GoToDefendIcon", velocity);
	}

	private void FixedUpdate()
	{
		if (!isPickedUp) return;

		Vector3 targetWorldPos = CalTargetWorldPos();
		if (Vector3.Distance(targetWorldPos, transform.position) < destoryDistance)
		{
			Destroy(gameObject);
			PanelLevelHUD.OnPickUpDefendTreasure(defendValue);
		}
		else
		{
			Vector2 targetVelocity = maxSpeed * ((targetWorldPos - transform.position).normalized);
			GetComponent<MoveManager>().ChangeTargetVelocity("_GoToDefendIcon", targetVelocity);
		}
	}

	private Vector3 CalTargetWorldPos()
	{
		Vector3 targetScreenPos = new Vector3(defendIconScreenPos.x, defendIconScreenPos.y, 0);
		Vector3 targetWorldPos = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>().ScreenToWorldPoint(targetScreenPos);
		targetWorldPos.z = 0;

		return targetWorldPos;
	}
}
