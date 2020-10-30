using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostReady : GhostState
{ 
    private GhostManager _ghostManager;

    public float WaitintForOutTime = 1.0f;
    private float _waitOutTimeTick;

    public override void Initialize(GhostManager ghostManager)
    {
        _ghostManager = ghostManager;
    }

    public override void Enter()
    {
        _waitOutTimeTick = 0.0f;
    }

    public override void Execute()
    {
        _waitOutTimeTick += _ghostManager.Delta;
        if(_waitOutTimeTick >= WaitintForOutTime)
        {
            _ghostManager.ChangeState(_ghostManager.GetGhostIdle());
        }
    }

    public override void Exit()
    {
    }
}
