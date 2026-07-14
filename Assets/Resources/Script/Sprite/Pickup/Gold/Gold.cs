using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Pickup
{
	private const float speed = 20f;
	private const float destoryDistance = 0.2f;
	[SerializeField] private int goldValue;
	private Vector2 goldIconScreenPos;

	protected override void OnPickUp()
	{
		goldIconScreenPos = PanelLevelHUD.GetGoldIconScreenPos();
	}

	private void FixedUpdate()
	{
		if (!isPickedUp) return;

		Vector3 targetScreenPos = new Vector3(goldIconScreenPos.x, goldIconScreenPos.y, 0);
		Vector3 targetWorldPos = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>().ScreenToWorldPoint(targetScreenPos);
		targetWorldPos.z = 0;
		if (Vector3.Distance(targetWorldPos, transform.position) < destoryDistance)
		{
			Destroy(gameObject);
			PanelLevelHUD.OnPickUpGold(goldValue);
		}
		else
		{
			GetComponent<Rigidbody2D>().velocity = speed * ((targetWorldPos - transform.position).normalized);
		}
	}
}
