using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanIdle : PacmanState
{
    public static PacmanIdle Instance;

    private PacmanManager _pacmanManager;

    public override void Initialize(PacmanManager pacmanManager)
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _pacmanManager = pacmanManager;
    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
        if(_pacmanManager.GetInputData().Vertical != 0 || _pacmanManager.GetInputData().Horizontal != 0)
        {
            _pacmanManager.ChangeState(PacmanMovement.Instance);
        }
    }

    public override void Exit()
    {
    }
}
