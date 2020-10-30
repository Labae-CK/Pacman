using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAvoid : GhostState
{
    private GhostManager _ghostManager;
    private MapGenerator _mapGenerator;
    private Transform _transform;
    private Animator _animator;
    private PathFinder _pathFinder;
    private int _movePointIndex;

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
        Vector2Int targetIndexPos = GetTargetIndexPos();

        _animator.SetBool("Avoid", true);

        TileBase.AStarValues[,] aStarValues = _mapGenerator.GetAStarValues();
        Vector2Int startIndexPos = new Vector2Int(_ghostManager.GetCurrentTile().IndexPosX, _ghostManager.GetCurrentTile().IndexPosY);
        _pathFinder.PathFind(aStarValues, aStarValues[startIndexPos.x, startIndexPos.y], aStarValues[targetIndexPos.x, targetIndexPos.y]);
    }

    private Vector2Int GetTargetIndexPos()
    {
        Vector2 pacmanDir = (_ghostManager.GetPacman().transform.position - _transform.position).normalized;
        int avoidRange = Random.Range(5, 10);
        Vector2 avoidDir = pacmanDir * -Vector2.one * avoidRange;
        Vector2Int mapSize = _mapGenerator.MapSize;

        Vector2Int currentIndexPos = new Vector2Int(_ghostManager.GetCurrentTile().IndexPosX, _ghostManager.GetCurrentTile().IndexPosY);
        Vector2Int targetIndexPos = new Vector2Int(currentIndexPos.x + (int)avoidDir.x, currentIndexPos.y + (int)avoidDir.y);

        if (targetIndexPos.x <= 0)
        {
            targetIndexPos.x = 0;
        }
        if (targetIndexPos.y <= 0)
        {
            targetIndexPos.y = 0;
        }
        if (targetIndexPos.x >= mapSize.x)
        {
            targetIndexPos.x = mapSize.x - 1;
        }
        if (targetIndexPos.y >= mapSize.y)
        {
            targetIndexPos.y = mapSize.y - 1;
        }

        _movePointIndex = 0;
        return targetIndexPos;
    }

    public override void Execute()
    {
        if (_ghostManager.GetPacman().IsHunting == false)
        {
            _ghostManager.ChangeState(_ghostManager.GetGhostIdle());
            return;
        }

        TileBase.AStarValues[] path = _pathFinder.GetPath();
        if (path == null)
        {
            return;
        }

        if (path.Length <= _movePointIndex)
        {
            TileBase.AStarValues[,] aStarValues = _mapGenerator.GetAStarValues();
            Vector2Int startIndexPos = new Vector2Int(_ghostManager.GetCurrentTile().IndexPosX, _ghostManager.GetCurrentTile().IndexPosY);
            Vector2Int targetIndexPos = GetTargetIndexPos();
            _pathFinder.PathFind(aStarValues, aStarValues[startIndexPos.x, startIndexPos.y], aStarValues[targetIndexPos.x, targetIndexPos.y]);
            return;
        }

        if (path.Length > 0)
        {
            Vector3 targetPos = path[_movePointIndex].Tile.transform.position;
            if (targetPos == _transform.position)
            {
                _movePointIndex++;
                if (path.Length <= _movePointIndex)
                { 
                    return;
                }

                targetPos = path[_movePointIndex].Tile.transform.position;
            }
            _transform.position = Vector3.MoveTowards(_transform.position, targetPos, _ghostManager.Delta * _ghostManager.MoveSpeed);
        }
    }

    public override void Exit()
    {
        _animator.SetBool("Avoid", false);
    }
}
