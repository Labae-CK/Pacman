using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanHunting : PacmanState
{
    public static PacmanHunting Instance;

    public float HuntingTime = 5.0f;
    private float _huntingTickTime;

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
        _huntingTickTime = 0.0f;
        _pacmanManager.IsHunting = true;
        _audioSource.clip = _audioClip;
        _audioSource.loop = true;
        _audioSource.Play();

        _animator.SetBool("Move", true);
    }

    public override void Execute()
    {
        _huntingTickTime += _pacmanManager.Delta;
        if(_huntingTickTime >= HuntingTime)
        {
            _pacmanManager.ChangeState(PacmanIdle.Instance);
            return;
        }

        PacmanInput.InputData inputData = _pacmanManager.GetInputData();
        Vector2 dir = new Vector2(inputData.Horizontal, inputData.Vertical);
        if (dir == Vector2.zero)
        {
            _animator.SetBool("Move", false);
            return;
        }

        _animator.SetBool("Move", true);
        _pacmanManager.SetRotate(inputData);
        _transform.Translate(dir * _pacmanManager.HuntingSpeed * _pacmanManager.Delta);
    }

    public override void Exit()
    {
        _pacmanManager.IsHunting = false;
        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.clip = null;
        _animator.SetBool("Move", false);
    }
}
