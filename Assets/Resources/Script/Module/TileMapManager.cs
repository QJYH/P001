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

	private static bool HaveBlockOnCell(Vector3Int cellPos)
	{
		Vector3 worldPos = CellToWorld(cellPos);
		LayerMask obstacleLayer = LayerMask.GetMask("EnvironmentBlock", "Edge");
		Collider2D hit = Physics2D.OverlapCircle(worldPos, 0.45f, obstacleLayer);
		return hit;
	}

	private static bool HaveBlockOnLine(Vector3 beginWorldPos, Vector3 endWorldPos)
	{
		LayerMask obstacleLayer = LayerMask.GetMask("EnvironmentBlock", "Edge");
		RaycastHit2D hit = Physics2D.Linecast(beginWorldPos, endWorldPos, obstacleLayer);
		return hit;
	}

	public static bool CanStandOnPosition(Vector3 worldPos)
	{
		Vector3Int cellPos = WorldToCell(worldPos);
		return IsTileSolid(cellPos) && (!HaveBlockOnCell(cellPos));
	}

	public static bool CanReachDirectly(Vector3 beginWorldPos, Vector3 endWorldPos)
	{
		return CanStandOnPosition(endWorldPos) && (!HaveBlockOnLine(beginWorldPos, endWorldPos));
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
