using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GhostDeath : GhostState
{
    private GhostManager _ghostManager;
    private MapGenerator _mapGenerator;
    private Transform _transform;
    private Animator _animator;
    private PathFinder _pathFinder;

    private int _movePointIndex;
    private BoxCollider2D _collider;
    private bool _arrived;

    private float _respawnTime = 1.0f;
    private float _respawnTick;

    public override void Initialize(GhostManager ghostManager)
    {
        _ghostManager = ghostManager;
        _transform = ghostManager.transform;
        _animator = _ghostManager.GetAnimator();
        _mapGenerator = GameManager.Instance.MapGenerator;
        _pathFinder = _ghostManager.GetPathFinder();
        _collider = GetComponent<BoxCollider2D>();
    }

    public override void Enter()
    {
        _collider.enabled = false;
        _ghostManager.Death = true;
        _arrived = false;
        _movePointIndex = 0;
        TileBase.AStarValues[,] aStarValues = _mapGenerator.GetAStarValues();
        Vector2Int currentIndexPos = new Vector2Int(_ghostManager.GetCurrentTile().IndexPosX, _ghostManager.GetCurrentTile().IndexPosY);
        TileBase tile = _ghostManager.GetStartTile();
        Vector2Int targetPos = new Vector2Int(tile.IndexPosX, tile.IndexPosY);
        _pathFinder.PathFind(aStarValues, aStarValues[currentIndexPos.x, currentIndexPos.y], aStarValues[targetPos.x, targetPos.y]);
        _animator.SetBool("Death", true);
    }

    public override void Execute()
    {
        if(_arrived)
        {
            _respawnTick += Time.deltaTime;
            if(_respawnTick >= _respawnTime)
            {
                _ghostManager.ChangeState(_ghostManager.GetGhostIdle());
                return;
            }
        }

        TileBase.AStarValues[] path = _pathFinder.GetPath();
        if (path == null)
        {
            return;
        }

        if (path.Length > 0 && !_arrived)
        {
            Vector3 targetPos = path[_movePointIndex].Tile.transform.position;
            if (targetPos == _transform.position)
            {
                _movePointIndex++;
                if (path.Length == _movePointIndex)
                {
                    _arrived = true;
                    return;
                }

                targetPos = path[_movePointIndex].Tile.transform.position;
            }
            _transform.position = Vector3.MoveTowards(_transform.position, targetPos, _ghostManager.Delta * _ghostManager.ReturnBoxSpeed);
        }
    }

    public override void Exit()
    {
        _animator.SetBool("Death", false);
        _collider.enabled = true;
        _ghostManager.Death = false;
        _respawnTick = 0.0f;
    }
}
