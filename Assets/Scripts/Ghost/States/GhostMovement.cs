using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : GhostState
{
    private GhostManager _ghostManager;
    private MapGenerator _mapGenerator;
    private Transform _transform;
    private Animator _animator;
    private PathFinder _pathFinder;

    private int _movePointIndex;
    private int _maxMovePointIndex;

    public override void Initialize(GhostManager ghostManager)
    {
        _ghostManager = ghostManager;
        _transform = ghostManager.transform;
        _animator = _ghostManager.GetAnimator();
        _mapGenerator = GameManager.Instance.MapGenerator;
        _pathFinder = _ghostManager.GetPathFinder();
    }

    public override void Enter()
    {
        SetRandomMovePoints();
    }

    private void SetRandomMovePoints()
    {
        _movePointIndex = 0;
        _maxMovePointIndex = Random.Range(3, 10);

        TileBase[,] mapData = _mapGenerator.TileDatas;
        Vector2Int mapSize = _mapGenerator.GetMapSize();
        int randX = 0;
        int randY = 0;
        while (true)
        {
            randX = Random.Range(0, mapSize.x);
            randY = Random.Range(0, mapSize.y);
            if(mapData[randX, randY].CanWalk)
            {
                break;
            }
        }

        TileBase.AStarValues[,] aStarValues = _mapGenerator.GetAStarValues();
        Vector2Int startIndexPos = new Vector2Int(_ghostManager.GetCurrentTile().IndexPosX, _ghostManager.GetCurrentTile().IndexPosY);
        _pathFinder.PathFind(aStarValues, aStarValues[startIndexPos.x, startIndexPos.y], aStarValues[randX, randY]);
    }

    public override void Execute()
    {
        TileBase.AStarValues[] path = _pathFinder.GetPath();
        if (path == null)
        {
            return;
        }

        if (path.Length > 0)
        {
            Vector3 targetPos = path[_movePointIndex].Tile.transform.position;
            if (targetPos == _transform.position)
            {
                _movePointIndex++;
                if (path.Length == _movePointIndex)
                {
                    _ghostManager.ChangeState(_ghostManager.GetGhostIdle());
                    return;
                }

                if (_movePointIndex == _maxMovePointIndex)
                {
                    _ghostManager.ChangeState(_ghostManager.GetGhostIdle());
                    return;
                }

                targetPos = path[_movePointIndex].Tile.transform.position;
            }
            _transform.position = Vector3.MoveTowards(_transform.position, targetPos, _ghostManager.Delta * _ghostManager.MoveSpeed);

            Vector2 dir = (targetPos - _transform.position).normalized;
            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
        }

        if (Vector2.Distance(_transform.position, _ghostManager.GetPacman().transform.position) < _ghostManager.FollowPacmanDistance)
        {
            if(!_ghostManager.GetPacman().IsHunting)
            {
                _ghostManager.ChangeState(_ghostManager.GetGhostFollowing());
            }
        }
    }

    public override void Exit()
    {
        _movePointIndex = 0;
    }
}
