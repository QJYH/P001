using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTreasure : Pickup
{
	private const float maxSpeed = 15f;
	private const float maxAcceleration = 100f;
	private const float destoryDistance = 0.5f;
	private const int attackValue = 1;

	private Vector2 attackIconScreenPos;

	protected override void OnPickUp(Hero hero)
	{
		hero.PickUpAttackTreasure(attackValue);

		attackIconScreenPos = PanelLevelHUD.GetAttackIconScreenPos();
		GetComponent<SpriteRenderer>().sortingLayerName = "UI";

		Vector3 targetWorldPos = CalTargetWorldPos();
		Vector2 targetVelocity = maxSpeed * ((targetWorldPos - transform.position).normalized);
		Velocity velocity = new Velocity(Vector2.zero, targetVelocity, maxAcceleration);
		GetComponent<MoveManager>().AddVelocity("_GoToAttackIcon", velocity);
	}

	private void FixedUpdate()
	{
		if (!isPickedUp) return;

		Vector3 targetWorldPos = CalTargetWorldPos();
		if (Vector3.Distance(targetWorldPos, transform.position) < destoryDistance)
		{
			Destroy(gameObject);
			PanelLevelHUD.OnPickUpAttackTreasure(attackValue);
		}
		else
		{
			Vector2 targetVelocity = maxSpeed * ((targetWorldPos - transform.position).normalized);
			GetComponent<MoveManager>().ChangeTargetVelocity("_GoToAttackIcon", targetVelocity);
		}
	}

	private Vector3 CalTargetWorldPos()
	{
		Vector3 targetScreenPos = new Vector3(attackIconScreenPos.x, attackIconScreenPos.y, 0);
		Vector3 targetWorldPos = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>().ScreenToWorldPoint(targetScreenPos);
		targetWorldPos.z = 0;

		return targetWorldPos;
	}
}
