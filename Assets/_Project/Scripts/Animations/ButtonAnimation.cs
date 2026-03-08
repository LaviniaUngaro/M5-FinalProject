using UnityEngine;
using UnityEngine.Events;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private string _paramIsPressed = "isPressed";
    [SerializeField] public UnityEvent _onButtonDown;

    private bool _isPressed;
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _isPressed = false;
    }

    [ContextMenu("ToggleButton")]
    public void ToggleButton()
    {
        if (_isPressed) return;

        _isPressed = true;

        OnButtonPressed();
        _onButtonDown.Invoke(); // apre il cancello
    }

    public void OnButtonPressed()
    {
        _animator.SetTrigger(_paramIsPressed);
    }
}
