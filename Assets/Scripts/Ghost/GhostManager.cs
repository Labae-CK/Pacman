using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    private GhostState _currentState;
    private PacmanManager _pacmanManager;
    private MapGenerator _mapGenerator;
    private Animator _animator;
    private PathFinder _pathFinder;

    private GhostMovement _ghostMovement;
    private GhostIdle _ghostIdle;
    private GhostReady _ghostReady;
    private GhostFollowing _ghostFollowing;
    private GhostAvoid _ghostAvoid;
    private GhostDeath _ghostDeath;

    private TileBase _currentTile;

    private TileBase _startTile;

    [HideInInspector]
    public float Delta;

    public bool Death;

    public float MoveSpeed = 1.0f;
    public float FollowingSpeed = 3.0f;
    public float FollowPacmanDistance = 5.0f;
    public float ReturnBoxSpeed = 6.0f;

    public void Initialize(PacmanManager pacmanManager)
    {
        _pacmanManager = pacmanManager;
        _mapGenerator = GameManager.Instance.MapGenerator;
        _pathFinder = GetComponent<PathFinder>();
        _pathFinder.Initialize();

        _animator = GetComponentInChildren<Animator>();

        GhostState[] States = GetComponents<GhostState>();
        for (int i = 0; i < States.Length; i++)
        {
            States[i].Initialize(this);
        }

        _startTile = GetCurrentTile();

        _currentState = GetComponent<GhostReady>();
        _currentState.Enter();
    }

    public void ChangeState(GhostState newState)
    {
        if (_currentState == newState)
        {
            return;
        }
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            return;
        }

        Delta = Time.deltaTime;
        if(_pacmanManager.IsHunting && _currentState != _ghostAvoid)
        {
            if(!Death)
            {
                ChangeState(GetGhostAvoid());
            }
        }

        if (_currentState != null)
        {
            _currentState.Execute();
        }
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public GhostReady GetGhostReady()
    {
        if(_ghostReady == null)
        {
            _ghostReady = GetComponent<GhostReady>();
        }

        return _ghostReady;
    }

    public GhostMovement GetGhostMovement()
    {
        if (_ghostMovement == null)
        {
            _ghostMovement = GetComponent<GhostMovement>();
        }

        return _ghostMovement;
    }

    public GhostIdle GetGhostIdle()
    {
        if (_ghostIdle == null)
        {
            _ghostIdle = GetComponent<GhostIdle>();
        }

        return _ghostIdle;
    }

    public GhostFollowing GetGhostFollowing()
    {
        if (_ghostFollowing == null)
        {
            _ghostFollowing = GetComponent<GhostFollowing>();
        }

        return _ghostFollowing;
    }

    public GhostAvoid GetGhostAvoid()
    {
        if (_ghostAvoid == null)
        {
            _ghostAvoid = GetComponent<GhostAvoid>();
        }

        return _ghostAvoid;
    }

    public GhostDeath GetGhostDeath()
    {
        if (_ghostDeath == null)
        {
            _ghostDeath = GetComponent<GhostDeath>();
        }

        return _ghostDeath;
    }

    public PacmanManager GetPacman()
    {
        return _pacmanManager;
    }

    public TileBase GetCurrentTile()
    {
        Vector2Int currentIndexPos = GameManager.GetIndexPosition(_mapGenerator, transform);
        _currentTile = _mapGenerator.TileDatas[currentIndexPos.x, currentIndexPos.y];

        return _currentTile;
    }

    public PathFinder GetPathFinder()
    {
        return _pathFinder;
    }

    public TileBase GetStartTile()
    {
        return _startTile;
    }
}
