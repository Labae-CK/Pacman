using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private MapGenerator _mapGenerator;

    private bool _pathSuccessFind;

    private TileBase.AStarValues[] _path;

    public void Initialize()
    {
        _mapGenerator = GameManager.Instance.MapGenerator;
    }

    public void PathFind(TileBase.AStarValues[,] aStarValues, TileBase.AStarValues start, TileBase.AStarValues end)
    {
        StartCoroutine(FindPath(aStarValues, start, end));
    }

    private IEnumerator FindPath(TileBase.AStarValues[,] aStarValues, TileBase.AStarValues start, TileBase.AStarValues end)
    {
        TileBase.AStarValues[] wayPoints = new TileBase.AStarValues[0];
        _pathSuccessFind = false;

        if (start.Tile.CanWalk && end.Tile.CanWalk)
        {
            Heap<TileBase.AStarValues> openSet = new Heap<TileBase.AStarValues>(_mapGenerator.MapSize.x * _mapGenerator.MapSize.y);
            HashSet<TileBase.AStarValues> closeSet = new HashSet<TileBase.AStarValues>();

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                TileBase.AStarValues currentTile = openSet.RemoveFirst();
                closeSet.Add(currentTile);

                if (currentTile == end)
                {
                    _pathSuccessFind = true;
                    break;
                }

                foreach (TileBase.AStarValues neighbour in _mapGenerator.GetNeighbours(aStarValues, currentTile))
                {
                    if (!neighbour.Tile.CanWalk || closeSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentTile.GetGCost() + GetDistance(currentTile, neighbour);
                    if (newMovementCostToNeighbour < neighbour.GetGCost() || !openSet.Contains(neighbour))
                    {
                        neighbour.SetGCost(newMovementCostToNeighbour);
                        neighbour.SetHCost(GetDistance(neighbour, end));
                        neighbour.SetParent(currentTile);

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        yield return null;
        if (_pathSuccessFind)
        {
            wayPoints = RetracePath(start, end);
            OnPathFound(wayPoints, true);
        }
    }

    private TileBase.AStarValues[] RetracePath(TileBase.AStarValues startTile, TileBase.AStarValues endTile)
    {
        List<TileBase.AStarValues> path = new List<TileBase.AStarValues>();
        TileBase.AStarValues currentNode = endTile;

        while (currentNode != startTile)
        {
            path.Add(currentNode);
            currentNode = currentNode.GetParent();
        }

        path.Reverse();
        path.Insert(0, startTile);

        return path.ToArray();
    }

    private void OnPathFound(TileBase.AStarValues[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            _path = newPath;
        }
    }

    public int GetDistance(TileBase.AStarValues tileA, TileBase.AStarValues tileB)
    {
        int distX = Mathf.Abs(tileA.Tile.IndexPosX - tileB.Tile.IndexPosX);
        int distY = Mathf.Abs(tileA.Tile.IndexPosY - tileB.Tile.IndexPosY);

        return distX + distY;
    }

    public TileBase.AStarValues[] GetPath()
    {
        return _path;
    }
}
