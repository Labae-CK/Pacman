using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PacmanState : MonoBehaviour
{
    public abstract void Initialize(PacmanManager pacmanManager);
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
