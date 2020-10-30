using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostIdle : GhostState
{
    private GhostManager _ghostManager;

    public override void Initialize(GhostManager ghostManager)
    {
        _ghostManager = ghostManager;
    }

    public override void Enter()
    {
        _ghostManager.ChangeState(_ghostManager.GetGhostMovement());
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
    }
}
