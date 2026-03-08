using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField] private string _paramSpeed = "speed";
    [SerializeField] private string _paramAttack = "attack";
    
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void SetSpeed(float speed)
    {
        _animator.SetFloat(_paramSpeed, speed);
    }
    
    public void Attack()
    {
        _animator.SetTrigger(_paramAttack);
    }
}