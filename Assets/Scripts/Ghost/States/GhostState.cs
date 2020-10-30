using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostState : MonoBehaviour
{
    public abstract void Initialize(GhostManager ghostManager);
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
