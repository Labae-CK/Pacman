using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMovement : PacmanState
{
    public static PacmanMovement Instance;

    private PacmanManager _pacmanManager;
    private Transform _transform;
    private Animator _animator;

    public AudioClip _audioClip;
    private AudioSource _audioSource;

    public override void Initialize(PacmanManager pacmanManager)
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _pacmanManager = pacmanManager;
        _transform = transform;
        _animator = _pacmanManager.GetAnimator();
        _audioSource = _pacmanManager.GetAudioSource();
    }

    public override void Enter()
    {
        _animator.SetBool("Move", true);
        _audioSource.clip = _audioClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public override void Execute()
    {
        PacmanInput.InputData inputData = _pacmanManager.GetInputData();
        Vector2 dir = new Vector2(inputData.Horizontal, inputData.Vertical);
        if (dir == Vector2.zero)
        {
            _pacmanManager.ChangeState(PacmanIdle.Instance);
            return;
        }

        _pacmanManager.SetRotate(inputData);
        _transform.Translate(dir * _pacmanManager.MoveSpeed * _pacmanManager.Delta);
    }

    public override void Exit()
    {
        _animator.SetBool("Move", false);
        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.clip = null;
    }
}
