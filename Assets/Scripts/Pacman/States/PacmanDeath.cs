using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanDeath : PacmanState
{
    public static PacmanDeath Instance;

    private PacmanManager _pacmanManager;

    public override void Initialize(PacmanManager pacmanManager)
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _pacmanManager = pacmanManager;
    }

    public override void Enter()
    {
        // 죽는 애니메이션 출력.
        GameManager.Instance.IsGameOver = true;
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        // 게임 종료 or 다시 시작
    }
}
