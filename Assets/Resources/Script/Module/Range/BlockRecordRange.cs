using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class BlockRecordRange : Range
{
	private const int blockTextureBlurTimes = 8;
	private const float updateBlockTextureInterval = 0.1f;


	private Dictionary<int, GameObject> colliderDict = new Dictionary<int, GameObject>();
	private Texture2D blockTexture;
	private Camera mainCamera;
	private float lastUpdateBlockTextureTime = 0;
	private int savedTimes = 0;

	// Start is called before the first frame update
	void Start()
    {
		mainCamera = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>();
		SetColliderSize();
		InitBlockTexture();
	}

	private void InitBlockTexture()
	{
		blockTexture = new Texture2D(Screen.width / blockTextureBlurTimes, Screen.height / blockTextureBlurTimes);
	}

	private void SetColliderSize()
	{
		Vector3 bottomLeftWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 topRightWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		GetComponent<BoxCollider2D>().size = new Vector2((topRightWorldPos.x - bottomLeftWorldPos.x), (topRightWorldPos.y - bottomLeftWorldPos.y));
	}

	// Update is called once per frame
	void Update()
    {
		FollowCamera();
		UpdateBlockTexture();
    }

	private void UpdateBlockTexture()
	{  
		if (Time.time - lastUpdateBlockTextureTime < updateBlockTextureInterval) return;
		lastUpdateBlockTextureTime = Time.time;

		int textureWidth = blockTexture.width;
		int textureHeight = blockTexture.height;
		for (int x = 0; x < textureWidth; x++)
		{
			for (int y = 0; y < textureHeight; y++)
			{
				blockTexture.SetPixel(x, y, Color.white);
			}
		}
		foreach (var keyValuePair in colliderDict)
		{
			Vector3 gameObjectPos = keyValuePair.Value.transform.position;
			BoxCollider2D collider2D = keyValuePair.Value.GetComponent<BoxCollider2D>();
			Vector3 bottomLeftWorldPos = new Vector3(gameObjectPos.x + collider2D.offset.x - collider2D.size.x / 2, gameObjectPos.y + collider2D.offset.y - collider2D.size.y / 2, 0);
			Vector3 topRightWorldPos = new Vector3(gameObjectPos.x + collider2D.offset.x + collider2D.size.x / 2, gameObjectPos.y + collider2D.offset.y + collider2D.size.y / 2, 0);
			Vector3 bottomLeftScreenPos = mainCamera.WorldToScreenPoint(bottomLeftWorldPos);
			Vector3 topRightScreenPos = mainCamera.WorldToScreenPoint(topRightWorldPos);
			Vector2Int bottomLeftTexturePos = new Vector2Int((int)(bottomLeftScreenPos.x / blockTextureBlurTimes), (int)(bottomLeftScreenPos.y / blockTextureBlurTimes));
			Vector2Int topRightTexturePos = new Vector2Int((int)(topRightScreenPos.x / blockTextureBlurTimes), (int)(topRightScreenPos.y / blockTextureBlurTimes));

			for (int x = bottomLeftTexturePos.x; x <= topRightTexturePos.x; x++)
			{
				for (int y = bottomLeftTexturePos.y; y <= topRightTexturePos.y; y++)
				{
					if ((x < 0) || (x >= textureWidth)) continue;
					if ((y < 0) || (y >= textureHeight)) continue;

					blockTexture.SetPixel(x, y, Color.black);
				}
			}
		}
		blockTexture.Apply();

		//if (savedTimes < 0)
		//{
		//	savedTimes += 1;
		//	string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
		//	string fullPath = Path.Combine(desktopPath, "blocktexture.png");
		//	byte[] bytes = ImageConversion.EncodeToPNG(blockTexture);
		//	File.WriteAllBytes(fullPath, bytes);
		//}
	}

	private void FollowCamera()
	{
		Vector3 cameraPos = SceneDirector.SceneInstance.MainCamera.transform.position;
		transform.position = new Vector3(cameraPos.x, cameraPos.y, 0);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision is not BoxCollider2D) return;
		if (LayerMask.LayerToName(collision.gameObject.layer) != "EnvironmentBlock") return;

		colliderDict.Add(collision.gameObject.GetInstanceID(), collision.gameObject);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		colliderDict.Remove(collision.gameObject.GetInstanceID());
	}
}
