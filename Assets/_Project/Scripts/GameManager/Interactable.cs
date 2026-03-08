using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent _onInteract;

    public void Interact()
    {
        _onInteract?.Invoke();
    }
}
