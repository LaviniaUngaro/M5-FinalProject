using UnityEngine;

public class StadingStillEnemy : EnemiesFSM
{
    [Header("Patrol Attributes")]
    [SerializeField] private float _rotationInterval = 6f;
    [SerializeField] private float _rotationSpeed = 180;

    private float _timer;
    private float _remainingRotation;

    protected override void PatrolUpdate()
    {
        _timer += Time.deltaTime;

        if (_timer >= _rotationInterval && _remainingRotation <= 0f)
        {
            _timer = 0f;
            _remainingRotation = 180f;
        }

        if (_remainingRotation > 0f)
        {
            float degreesThisFrame = _rotationSpeed * Time.deltaTime;
            float rotationThisFrame = Mathf.Min(degreesThisFrame, _remainingRotation);

            transform.Rotate(0, rotationThisFrame, 0);
            _remainingRotation -= rotationThisFrame;
        }

        base.PatrolUpdate();
    }
}
