using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFollowing : GhostState
{
    private GhostManager _ghostManager;
    private Transform _transform;
    private Animator _animator;
    private MapGenerator _mapGenerator;
    private PathFinder _pathFinder;
    private PacmanManager _pacman;

    [SerializeField]
    private TileBase.AStarValues[] _path;
    private int _movePointIndex = 0;

    private Vector2Int _prevPacmanIndexPos;

    public override void Initialize(GhostManager ghostManager)
    {
        _ghostManager = ghostManager;
        _transform = ghostManager.transform;
        _animator = _ghostManager.GetAnimator();
        _mapGenerator = GameManager.Instance.MapGenerator;
        _pathFinder = _ghostManager.GetPathFinder();
        _pacman = _ghostManager.GetPacman();
    }

    public override void Enter()
    {
        TileBase.AStarValues[,] aStarValues = _mapGenerator.GetAStarValues();
        Vector2Int currentIndexPos = new Vector2Int(_ghostManager.GetCurrentTile().IndexPosX, _ghostManager.GetCurrentTile().IndexPosY);
        _pathFinder.PathFind(aStarValues, aStarValues[currentIndexPos.x, currentIndexPos.y], aStarValues[_pacman.CurrentPosIndex.x, _pacman.CurrentPosIndex.y]);
        _movePointIndex = 0;
        _prevPacmanIndexPos = _pacman.CurrentPosIndex;

        _path = _pathFinder.GetPath();
    }

    public override void Execute()
    {
        if (Vector2.Distance(_transform.position, _ghostManager.GetPacman().transform.position) > _ghostManager.FollowPacmanDistance)
        {
            _ghostManager.ChangeState(_ghostManager.GetGhostIdle());
            return;
        }

        if (_path == null)
        {
            return;
        }

        if(_pacman.CurrentPosIndex != _prevPacmanIndexPos)
        {
            TileBase.AStarValues[,] aStarValues = _mapGenerator.GetAStarValues();
            Vector2Int currentIndexPos = new Vector2Int(_ghostManager.GetCurrentTile().IndexPosX, _ghostManager.GetCurrentTile().IndexPosY);
            _pathFinder.PathFind(aStarValues, aStarValues[currentIndexPos.x, currentIndexPos.y], aStarValues[_pacman.CurrentPosIndex.x, _pacman.CurrentPosIndex.y]);
            _movePointIndex = 0;
            _prevPacmanIndexPos = _pacman.CurrentPosIndex;

            _path = _pathFinder.GetPath();
        }

        if (_path.Length <= _movePointIndex)
        {
            _pacman.ChangeState(PacmanDeath.Instance);
            return;
        }

        if (_path.Length > 0)
        {
            Vector3 targetPos = _path[_movePointIndex].Tile.transform.position;
            if(targetPos == _transform.position)
            {
                _movePointIndex++;
                if(_path.Length <= _movePointIndex)
                {
                    return;
                }
                targetPos = _path[_movePointIndex].Tile.transform.position;
            }

            _transform.position = Vector3.MoveTowards(_transform.position, targetPos, _ghostManager.Delta * _ghostManager.FollowingSpeed);
            Vector2 dir = (targetPos - _transform.position).normalized;
            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
        }
    }

    public override void Exit()
    {
    }
}
