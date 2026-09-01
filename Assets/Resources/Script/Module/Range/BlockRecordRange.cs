using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEngine;

public class BlockRecordRange : Range
{
	private const int textureBlurTimes = 4;
	private const float updateBlockTextureInterval = 0f;


	private Dictionary<int, GameObject> colliderDict = new Dictionary<int, GameObject>();
	private Texture2D blockTexture;
	private Camera mainCamera;
	private float lastUpdateBlockTextureTime = 0;

	// Start is called before the first frame update
	void Start()
    {
		mainCamera = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>();
		SetColliderSize();
		InitBlockTexture();
	}

	private void InitBlockTexture()
	{
		blockTexture = new Texture2D(Screen.width / textureBlurTimes, Screen.height / textureBlurTimes, TextureFormat.RGBA32, false, true);
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
		UpdateHeroPosition();
    }

	private void UpdateHeroPosition()
	{
		GameObject hero = LevelDirector.LevelInstance.CurrentHero;
		GameObject mainCamera = SceneDirector.SceneInstance.MainCamera;
		Vector3 screenPos = mainCamera.GetComponent<Camera>().WorldToScreenPoint(hero.transform.position);
		Vector2 heroScreenPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
		Resources.Load<Material>("Material/Enemy").SetVector("_HeroScreenPos", heroScreenPos);
	}

	private void UpdateBlockTexture()
	{  
		if (Time.time - lastUpdateBlockTextureTime < updateBlockTextureInterval) return;
		lastUpdateBlockTextureTime = Time.time;

		int textureWidth = blockTexture.width;
		int textureHeight = blockTexture.height;
		Color32 white = new Color32(255, 255, 255, 255);
		Color32 black = new Color32(0, 0, 0, 0);
		var rawTextureData = blockTexture.GetRawTextureData<Color32>();
		for (int i = 0; i < rawTextureData.Length; i++)
		{
			rawTextureData[i] = white;
		}
	
		foreach (var keyValuePair in colliderDict)
		{
			Vector3 gameObjectPos = keyValuePair.Value.transform.position;
			BoxCollider2D collider2D = keyValuePair.Value.GetComponent<BoxCollider2D>();
			Vector3 bottomLeftWorldPos = new Vector3(gameObjectPos.x + collider2D.offset.x - collider2D.size.x / 2, gameObjectPos.y + collider2D.offset.y - collider2D.size.y / 2, 0);
			Vector3 topRightWorldPos = new Vector3(gameObjectPos.x + collider2D.offset.x + collider2D.size.x / 2, gameObjectPos.y + collider2D.offset.y + collider2D.size.y / 2, 0);
			Vector3 bottomLeftScreenPos = mainCamera.WorldToScreenPoint(bottomLeftWorldPos);
			Vector3 topRightScreenPos = mainCamera.WorldToScreenPoint(topRightWorldPos);
			Vector2Int bottomLeftTexturePos = new Vector2Int((int)(bottomLeftScreenPos.x / textureBlurTimes), (int)(bottomLeftScreenPos.y / textureBlurTimes));
			Vector2Int topRightTexturePos = new Vector2Int((int)(topRightScreenPos.x / textureBlurTimes), (int)(topRightScreenPos.y / textureBlurTimes));

			int minY = Mathf.Max(bottomLeftTexturePos.y, 0);
			int maxY = Mathf.Min(topRightTexturePos.y, textureHeight - 1);
			int minX = Mathf.Max(bottomLeftTexturePos.x, 0);
			int maxX = Mathf.Min(topRightTexturePos.x, textureWidth - 1);
			for (int y = minY; y <= maxY; y++)
			{
				for (int x = minX; x <= maxX; x++)
				{
					rawTextureData[y * textureWidth + x] = black;
				}
			}
		}
		blockTexture.Apply();
		Material enemy = Resources.Load<Material>("Material/Enemy");
		enemy.SetTexture("_BlockTexture2D", blockTexture);

		//SaveTexture(blockTexture);
	}

	private void SaveTexture(Texture2D blockTexture)
	{
		string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
		string fullPath = Path.Combine(desktopPath, "blocktexture.png");
		byte[] bytes = ImageConversion.EncodeToPNG(blockTexture);
		File.WriteAllBytes(fullPath, bytes);
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
