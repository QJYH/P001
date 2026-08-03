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

	private static bool IsTileSolid(Vector3Int cellPos)
	{
		foreach (var gameObject in Instance.roadTileMapList)
		{
			Tilemap tilemap = gameObject.GetComponent<Tilemap>();
			TileBase tile = tilemap.GetTile(cellPos);
			if (tile != null)
			{
				return true;
			}
		}

		foreach (var gameObject in Instance.groundTileMapList)
		{
			Tilemap tilemap = gameObject.GetComponent<Tilemap>();
			TileBase tile = tilemap.GetTile(cellPos);
			if (tile != null)
			{
				return true;
			}
		}

		return false;
	}

	private static bool HaveBlockOnPosition(Vector2 worldPos, Vector2 collisionOffset, Vector2 collisionSize)
	{
		LayerMask obstacleLayer = LayerMask.GetMask("EnvironmentBlock", "Edge");
		Collider2D hit = Physics2D.OverlapBox(worldPos + collisionOffset, collisionSize, 0, obstacleLayer);
		return hit;
	}

	private static bool HaveBlockOnLine(Vector2 beginWorldPos, Vector2 endWorldPos, Vector2 collisionOffset, Vector2 collisionSize)
	{
		LayerMask obstacleLayer = LayerMask.GetMask("EnvironmentBlock", "Edge");
		Vector2 direction = (endWorldPos - beginWorldPos).normalized;
		float distance = Vector2.Distance(beginWorldPos, endWorldPos);
		RaycastHit2D hit = Physics2D.BoxCast(beginWorldPos + collisionOffset, collisionSize, 0, direction, distance, obstacleLayer);
		return hit;
	}

	public static bool CanStandOnPosition(Vector3 worldPos, Vector2 collisionOffset, Vector2 collisionSize)
	{
		Vector3Int cellPos = WorldToCell(worldPos);
		return IsTileSolid(cellPos) && (!HaveBlockOnPosition(worldPos, collisionOffset, collisionSize));
	}

	public static bool CanReachDirectly(Vector3 beginWorldPos, Vector3 endWorldPos, Vector2 collisionOffset, Vector2 collisionSize)
	{
		bool canStandOnPosition = CanStandOnPosition(endWorldPos, collisionOffset, collisionSize);
		bool haveBlockOnLine = HaveBlockOnLine(beginWorldPos, endWorldPos, collisionOffset, collisionSize);
		return canStandOnPosition && (!haveBlockOnLine);
	}

	public static Vector3Int WorldToCell(Vector3 worldPosition)
	{
		GameObject groundTileMap = Instance.groundTileMapList[1];
		Tilemap tilemap = groundTileMap.GetComponent<Tilemap>();
		return tilemap.WorldToCell(worldPosition);
	}

	public static Vector3 CellToWorld(Vector3Int cellPosition)
	{
		GameObject groundTileMap = Instance.groundTileMapList[1];
		Tilemap tilemap = groundTileMap.GetComponent<Tilemap>();
		return tilemap.GetCellCenterWorld(cellPosition);
	}

	internal static bool IsInSameTile(Vector3 beginWorldPos, Vector3 endWorldPos)
	{
		Vector3Int beginCellPos = WorldToCell(beginWorldPos);
		Vector3Int endCellPos = WorldToCell(endWorldPos);
		return beginCellPos == endCellPos;
	}
}
