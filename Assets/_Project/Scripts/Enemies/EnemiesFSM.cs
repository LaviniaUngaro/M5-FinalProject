using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesFSM : MonoBehaviour
{
    public enum STATE { PATROL, CHASE, ATTACK, COOLDOWN }

    [Header("States Attributes")]
    [SerializeField] private STATE _currentState;
    [SerializeField] private Transform _target;

    [Header("Chase Attributes")]
    [SerializeField] private float _updatePathInterval = 0.5f;
    [SerializeField] private float _attackDistance = 3f;

    [Header("Animations")]
    [SerializeField] private CharacterAnimations _characterAnimCon;
    [SerializeField] private float _patrolSpeed = 1.5f;
    [SerializeField] private float _chaseSpeed = 3.5f;

    // POSITIONS
    private float _lastPathUpdateTime;
    private Vector3 _lastPositionKnown;
    private Vector3 _enemyStartPosition;

    // REFERENCES
    protected NavMeshAgent _agent;
    private TargetDetection _targetDetection;
    private GameManager _gameManager;
    private PlayerController _player;

    #region PROPERTY
    public Transform Target => _target;
    public NavMeshAgent Agent => _agent;
    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _targetDetection = GetComponent<TargetDetection>();
        _gameManager = FindObjectOfType<GameManager>();
        _player = FindObjectOfType<PlayerController>();
        _enemyStartPosition = transform.position;
        if (_characterAnimCon == null) _characterAnimCon = GetComponent<CharacterAnimations>();

        SetState(STATE.PATROL); // setto lo stato iniziale a Patrol
    }

    private void Update()
    {
        switch (_currentState)
        {
            case STATE.PATROL:
                PatrolUpdate();
                break;
            case STATE.CHASE:
                ChaseUpdate();
                break;
            case STATE.ATTACK:
                AttackUpdate();
                break;
            case STATE.COOLDOWN:
                CooldownUpdate();
                break;
        }
        _characterAnimCon.SetSpeed(_agent.velocity.magnitude);
    }

    #region Funzioni SetState, OnExitState, OnEnterState
    public void SetState(STATE state)
    {
        ExitState();
        _currentState = state;
        EnterState();
    }

    private void ExitState()
    {
        switch (_currentState)
        {
            case STATE.PATROL:
                OnExitPatrol();
                break;
        }
    }

    protected virtual void OnExitPatrol() { }


    private void EnterState()
    {
        switch (_currentState)
        {
            case STATE.PATROL:
                OnEnterPatrol();
                break;
            case STATE.CHASE:
                OnEnterChase();
                break;
            case STATE.ATTACK:
                OnEnterAttack();
                break;
            case STATE.COOLDOWN:
                OnEnterCooldown();
                break;
        }
    }

    protected virtual void OnEnterPatrol()
    {
        _agent.speed = _patrolSpeed;
    }

    private void OnEnterChase()
    {
        SoundManager.Instance.OnEnemiesAlert();
        _agent.speed = _chaseSpeed;

        _lastPathUpdateTime = 0f;
        _agent.SetDestination(_target.position);
    }

    private void OnEnterAttack()
    {
        _characterAnimCon.Attack();
        _player.DisableMovement();
        _gameManager.GameOver();
    }

    private void OnEnterCooldown()
    {
        _agent.speed = _patrolSpeed;
        _agent.SetDestination(_enemyStartPosition);
    }
    #endregion


    #region Funzioni StateUpdate
    protected virtual void PatrolUpdate()
    {
        if (_targetDetection.CanSeeTarget())
        {
            SetState(STATE.CHASE);
        }
    }

    private void ChaseUpdate()
    {
        if (_targetDetection.CanSeeTarget()) // se lo vedo lo inseguo e aggiorno l'ultima posizione nota
        {
            if (Time.time - _lastPathUpdateTime > _updatePathInterval)
            {
                Agent.SetDestination(_target.position);
                _lastPathUpdateTime = Time.time;
                _lastPositionKnown = _target.position;
            }
        }
        else // se non lo vedo raggiungo l'ultima posizione nota
        {
            _agent.SetDestination(_lastPositionKnown);

            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                SetState(STATE.COOLDOWN);
            }
        }

        if (CanAttack()) // se lo raggiungo lo attacco
        {
            SetState(STATE.ATTACK);
        }
    }

    private void AttackUpdate() // se non posso attaccare continuo a inseguirlo
    {
        if (!CanAttack())
        {
            SetState(STATE.CHASE);
        }
    }

    private void CooldownUpdate() // se ho raggiunto l'ultima posizione nota torno a fare patrol
    {
        if (_targetDetection.CanSeeTarget())
        {
            SetState(STATE.CHASE);
            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            SetState(STATE.PATROL);
        }
    }
    #endregion

    private bool CanAttack()
    {
        if (_target == null) return false;

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _attackDistance) return true;

        return false;
    }
}