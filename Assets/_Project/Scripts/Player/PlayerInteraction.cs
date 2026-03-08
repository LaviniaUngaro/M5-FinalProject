using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _playerReach = 3f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private Interactable[] _buttons;

    [SerializeField] private UnityEvent _whenNearButton;
    [SerializeField] private UnityEvent _whenNotNearButton;

    private Interactable _currentButton;
    private bool isCloseEnoughToButton;
    private float _closestDistance;
    private NavMeshAgent _agent;

    void Awake()
    {
        if (_buttons == null) _buttons = FindObjectsOfType<Interactable>();
        _currentButton = GetComponent<Interactable>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_buttons == null) return;

        ShowButtonUI();

        if (Input.GetKeyDown(KeyCode.E) && isCloseEnoughToButton)
        {
            _agent.SetDestination(_currentButton.transform.position);
            _currentButton.Interact();
            Debug.Log("Il player ha premuto il bottone");
        }
    }

    private bool ShowButtonUI()
    {
        _closestDistance = Mathf.Infinity;
        _currentButton = null;

        foreach (Interactable button in _buttons)
        {
            float distance = Vector3.Distance(transform.position, button.transform.position);

            if (distance <= _closestDistance)
            {
                _closestDistance = distance;
                _currentButton = button;
            }
        }

        if (_closestDistance <= _playerReach && _currentButton != null)
        {
            isCloseEnoughToButton = true;
            _whenNearButton.Invoke();
            return true;
        }
        else
        {
            isCloseEnoughToButton = false;
            _whenNotNearButton.Invoke();
            return false;
        }
    }
}