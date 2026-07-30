using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
	public static TileMapManager Instance { get; private set; }

	private List<GameObject> roadTileMapList = new List<GameObject>();
	private List<GameObject> groundTileMapList = new List<GameObject>();

	private void Awake()
	{
		Instance = this;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void OnEnable()
	{
		RoadTileMap.OnRoadTileMapStart.AddListener(OnRoadTileMapStart);
		GroundTileMap.OnGroundTileMapStart.AddListener(OnGroundTileMapStart);
	}

	private void OnDisable()
	{
		RoadTileMap.OnRoadTileMapStart.RemoveListener(OnRoadTileMapStart);
		GroundTileMap.OnGroundTileMapStart.RemoveListener(OnGroundTileMapStart);
	}

	private void OnRoadTileMapStart(GameObject roadTileMap)
	{
		roadTileMapList.Add(roadTileMap);
	}

	private void OnGroundTileMapStart(GameObject groundTileMap)
	{
		groundTileMapList.Add(groundTileMap);
	}

	public static bool CanStandOnPosition(Vector3 worldPos)
	{
		return IsTileSolid(worldPos) && (!HaveBlockOnPosition(worldPos));
	}

	private static bool IsTileSolid(Vector3 worldPos)
	{
		foreach (var gameObject in Instance.roadTileMapList)
		{
			Tilemap tilemap = gameObject.GetComponent<Tilemap>();
			TileBase tile = tilemap.GetTile(tilemap.WorldToCell(worldPos));
			if (tile != null)
			{
				return true;
			}
		}

		foreach (var gameObject in Instance.groundTileMapList)
		{
			Tilemap tilemap = gameObject.GetComponent<Tilemap>();
			TileBase tile = tilemap.GetTile(tilemap.WorldToCell(worldPos));
			if (tile != null)
			{
				return true;
			}
		}

		return false;
	}

	private static bool HaveBlockOnPosition(Vector3 worldPos)
	{
		LayerMask obstacleLayer = LayerMask.GetMask("EnvironmentBlock", "Edge");
		Collider2D hit = Physics2D.OverlapCircle(worldPos, 0.45f, obstacleLayer);
		return hit;
	}

	public static bool CanReachDirectly(Vector3 beginWorldPos, Vector3 endWorldPos)
	{
		return CanStandOnPosition(endWorldPos) && (!HaveBlockOnLine(beginWorldPos, endWorldPos));
	}

	private static bool HaveBlockOnLine(Vector3 beginWorldPos, Vector3 endWorldPos)
	{
		LayerMask obstacleLayer = LayerMask.GetMask("EnvironmentBlock", "Edge");
		RaycastHit2D hit = Physics2D.Linecast(beginWorldPos, endWorldPos, obstacleLayer);
		return hit;
	}
}
