using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _rayDistance = 500f;

    [Header("References")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private CharacterAnimations _characterAnimCon;

    void Awake()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();
        if (_mainCamera == null) _mainCamera = Camera.main;
        if (_characterAnimCon == null) _characterAnimCon = GetComponent<CharacterAnimations>();
    }

    void Update()
    {
        float speed = _agent.velocity.magnitude;
        _characterAnimCon.SetSpeed(speed);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _ground))
            {
                _agent.SetDestination(hit.point);
                SoundManager.Instance.OnWalk();
            }
        }
    }

    public void DisableMovement()
    {
        if (_agent == null) return;

        _agent.isStopped = true;
        enabled = false;

        _characterAnimCon.SetSpeed(0);
    }
}
