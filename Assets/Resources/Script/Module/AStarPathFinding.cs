using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AStarPathFinding
{
	public class Node
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Node Parent { get; set; }

		public int G { get; set; }
		public int H { get; set; }
		public int F => G + H;

		public Node(int x, int y)
		{
			X = x;
			Y = y;
		}

	}

	public class AStar
	{
		public static List<Vector3> FindPath(Vector3 beginWorldPos, Vector3 endWorldPos)
		{
			Vector3Int beginCellPos = TileMapManager.WorldToCell(beginWorldPos);
			Vector3Int endCellPos = TileMapManager.WorldToCell(endWorldPos);
			Node startNode = new Node(beginCellPos.x, beginCellPos.y);
			Node endNode = new Node(endCellPos.x, endCellPos.y);

			List<Node> openList = new List<Node> { startNode };
			HashSet<Node> closedList = new HashSet<Node>();

			while (openList.Count > 0)
			{
				if (openList.Count > 50)
				{
					Debug.Log("???");
					return new List<Vector3> { beginWorldPos, endWorldPos };
				}
				Node currentNode = openList.OrderBy(n => n.F).First();

				if (currentNode == endNode)
					return ReconstructPath(currentNode);

				openList.Remove(currentNode);
				closedList.Add(currentNode);

				foreach (var neighborNode in GetNeighbors(currentNode))
				{
					if (!CanReachDirectly(currentNode, neighborNode) || closedList.Contains(neighborNode))
					{
						closedList.Add(neighborNode);
						continue;
					}

					int tentativeG = currentNode.G + 10;

					if (!openList.Contains(neighborNode) || tentativeG < neighborNode.G)
					{
						neighborNode.Parent = currentNode;
						neighborNode.G = tentativeG;
						neighborNode.H = Math.Abs(neighborNode.X - endNode.X) * 10 + Math.Abs(neighborNode.Y - endNode.Y) * 10;

						if (!openList.Contains(neighborNode))
							openList.Add(neighborNode);
					}
				}
			}

			return new List<Vector3> { beginWorldPos, endWorldPos };
		}

		private static List<Vector3> ReconstructPath(Node endNode)
		{
			List<Vector3> path = new List<Vector3>();
			Node currentNode = endNode;
			while (currentNode != null)
			{
				Vector3Int beginCellPos = new Vector3Int(currentNode.X, currentNode.Y, 0);
				Vector3 beginWorldPos = TileMapManager.CellToWorld(beginCellPos);
				path.Add(beginWorldPos);
				currentNode = currentNode.Parent;
			}
			path.Reverse();
			return path;
		}

		private static List<Node> GetNeighbors(Node node)
		{
			List<Node> neighbors = new List<Node>();
			int[] dx = { -1, 1, 0, 0 };
			int[] dy = { 0, 0, -1, 1 };

			for (int i = 0; i < 4; i++)
			{
				int nx = node.X + dx[i];
				int ny = node.Y + dy[i];
				Node neighbor = new Node(nx, ny);
				neighbors.Add(neighbor);
			}
			return neighbors;
		}

		private static bool CanReachDirectly(Node beginNode, Node endNode)
		{
			Vector3Int beginCellPos = new Vector3Int(beginNode.X, beginNode.Y, 0);
			Vector3Int endCellPos = new Vector3Int(endNode.X, endNode.Y, 0);
			Vector3 beginWorldPos = TileMapManager.CellToWorld(beginCellPos);
			Vector3 endWorldPos = TileMapManager.CellToWorld(endCellPos);

			return TileMapManager.CanReachDirectly(beginWorldPos, endWorldPos);
		}
	}
}

