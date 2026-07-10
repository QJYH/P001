using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveBlockRange : Range
{
	private Camera mainCamera;
	private List<PerspectiveBlock> perspectiveBlockList = new List<PerspectiveBlock>();
	private void Awake()
	{
		mainCamera = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>();
	}

	private void Update()
	{
		Vector3 heroScreenPos = mainCamera.WorldToScreenPoint(transform.position);

		foreach (var perspectiveBlock in perspectiveBlockList)
		{
			perspectiveBlock.SetHeroScreenPos(heroScreenPos);
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		PerspectiveBlock perspectiveBlock = collision.GetComponent<PerspectiveBlock>();
		if (perspectiveBlock == null) return;

		bool needAdd = true;
		foreach (var item in perspectiveBlockList)
		{
			if (item.GetInstanceID() == perspectiveBlock.GetInstanceID())
			{
				needAdd = false;
			}
		}
		if (needAdd)
		{
			perspectiveBlockList.Add(perspectiveBlock);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		PerspectiveBlock perspectiveBlock = collision.GetComponent<PerspectiveBlock>();
		if (perspectiveBlock == null) return;

		perspectiveBlock.SetHeroScreenPos(new Vector2(-Screen.width, -Screen.height));
		foreach (var item in perspectiveBlockList)
		{
			if(item.GetInstanceID() == perspectiveBlock.GetInstanceID())
			{
				perspectiveBlockList.Remove(item);
				return;
			}
		}
	}
}
