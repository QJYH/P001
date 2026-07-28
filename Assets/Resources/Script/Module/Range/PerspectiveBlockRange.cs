using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveBlockRange : Range
{
	private Camera mainCamera;
	private Dictionary<int, PerspectiveBlock> perspectiveBlockDict = new Dictionary<int, PerspectiveBlock>();

	private void Awake()
	{
		mainCamera = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>();
	}

	private void Update()
	{
		Vector3 heroScreenPos = mainCamera.WorldToScreenPoint(transform.position);

		foreach (var keyValuePair in perspectiveBlockDict)
		{
			if (keyValuePair.Value != null)
			{
				keyValuePair.Value.SetHeroScreenPos(heroScreenPos);
			}
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		PerspectiveBlock perspectiveBlock = collision.GetComponent<PerspectiveBlock>();
		if (perspectiveBlock == null) return;

		if (perspectiveBlockDict.ContainsKey(perspectiveBlock.GetInstanceID())) return;
		perspectiveBlockDict.Add(perspectiveBlock.GetInstanceID(), perspectiveBlock);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		PerspectiveBlock perspectiveBlock = collision.GetComponent<PerspectiveBlock>();
		if (perspectiveBlock == null) return;

		perspectiveBlock.SetHeroScreenPos(new Vector2(-Screen.width, -Screen.height));
		perspectiveBlockDict.Remove(perspectiveBlock.GetInstanceID());
	}
}
