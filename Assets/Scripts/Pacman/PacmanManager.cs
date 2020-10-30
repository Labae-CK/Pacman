using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PacmanManager : MonoBehaviour
{
    public float Power;

    private PacmanInput _pacmanInput;
    private PacmanState _currentState;

    private Animator _animator;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Transform _transform;

    [HideInInspector]
    public float Delta;
    public float MoveSpeed = 10.0f;
    public float HuntingSpeed = 3.0f;

    public float HuntingPower;
    private float _prevHuntingPower;

    public bool IsHunting = false;

    private Vector2Int _startPosIndex;
    public Vector2Int CurrentPosIndex;
    private float _tileOffset;
    private int _yOffset;

    public void Initialize(Vector2Int spawnPointIndex)
    {
        _startPosIndex = spawnPointIndex;
        _tileOffset = GameManager.Instance.MapGenerator.GetTileOffset();

        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _transform = transform;

        _pacmanInput = GetComponent<PacmanInput>();
        _pacmanInput.Initialize();

        PacmanState[] states = GetComponents<PacmanState>();
        for (int i = 0; i < states.Length; i++)
        {
            states[i].Initialize(this);
        }

        _prevHuntingPower = Power;
        _currentState = PacmanIdle.Instance;
        _currentState.Enter();

        _yOffset = Mathf.RoundToInt(_transform.position.y / _tileOffset);
    }

    public void ChangeState(PacmanState newState)
    {
        if(_currentState == newState)
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
        _pacmanInput.GetInput();
        if(_currentState != null)
        {
            _currentState.Execute();
        }

        CurrentPosIndex = GameManager.GetIndexPosition(GameManager.Instance.MapGenerator, _transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            Coin coin = collision.GetComponentInParent<Coin>();
            Power += coin.AddPowerValue;
            if(Power >= _prevHuntingPower + HuntingPower)
            {
                _prevHuntingPower = Power + HuntingPower;
                ChangeState(PacmanHunting.Instance);
            }
            coin.HandleDestroy();
        }

        if (collision.CompareTag("Ghost"))
        {
            if(IsHunting)
            {
                GhostManager _ghostManager = collision.GetComponent<GhostManager>();
                _ghostManager.ChangeState(_ghostManager.GetGhostDeath());
            }
            else
            {
                ChangeState(PacmanDeath.Instance);
            }
        }
    }

    public void SetRotate(PacmanInput.InputData inputData)
    {
        if (inputData.Horizontal != 0)
        {
            if (inputData.Horizontal > 0.0f)
            {
                _spriteRenderer.flipX = false;
                _spriteRenderer.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else
            {
                _spriteRenderer.flipX = true;
                _spriteRenderer.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        if (inputData.Vertical != 0)
        {
            _spriteRenderer.flipX = false;
            if (inputData.Vertical > 0.0f)
            {
                _spriteRenderer.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            }
            else
            {
                _spriteRenderer.transform.eulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
            }
        }
    }

    public PacmanInput.InputData GetInputData()
    {
        return _pacmanInput.GetInputData();
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }
}
