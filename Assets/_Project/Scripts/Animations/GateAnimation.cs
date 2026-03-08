using UnityEngine;
using UnityEngine.AI;

public class GateAnimation : MonoBehaviour
{
    [SerializeField] private NavMeshObstacle _gateNavMesh;
    [SerializeField] private string _paramOpenGate = "openGate";

    private Animator _animator;

    void Awake()
    {
        if (_gateNavMesh == null) _gateNavMesh = GetComponent<NavMeshObstacle>();
        _gateNavMesh.enabled = true;
        _animator = GetComponent<Animator>();
    }

    public void OpenGate()
    {
        _gateNavMesh.enabled = false;
        _animator.SetTrigger(_paramOpenGate);
        SoundManager.Instance.OnDoorsOpening();
    }
}
