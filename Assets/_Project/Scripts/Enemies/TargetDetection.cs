using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _target;
    [SerializeField] private LayerMask _obstacle;

    [Header("Vision Cone Settings")]
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private float _sightDistance = 8f;
    [SerializeField] private float _radius = 6f;
    [SerializeField] private int _segments = 12;
    [SerializeField] Color _normalColor = Color.blue;
    [SerializeField] Color _alertColor = Color.red;

    private LineRenderer _lineRenderer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = _segments + 2;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
    }

    void Update()
    {
        if (_target == null) return;

        UpdateVisionConeColor(); // decido colore del cono

        DrawVisionCone(); // disegno il cono in scena

        if (CanSeeTarget())
        {
            Debug.Log($"{gameObject.name} vede il target {_target.gameObject.name}");
        }
    }

    private void UpdateVisionConeColor()
    {
        if (CanSeeTarget())
        {
            _lineRenderer.startColor = _alertColor;
            _lineRenderer.endColor = _alertColor;
        }
        else
        {
            _lineRenderer.startColor = _normalColor;
            _lineRenderer.endColor = _normalColor;
        }
    }

    public bool CanSeeTarget()
    {
        Vector3 toTarget = _target.position - transform.position;
        float distanceToTarget = toTarget.sqrMagnitude;

        // se il player è a una distanza maggiore della capacità visiva del nemico -> non lo vede
        if (distanceToTarget > _sightDistance * _sightDistance) return false;

        // altrimenti
        toTarget.Normalize();

        // verifica matematica per determinare se un oggetto si trova fuori dal campo visivo di un altro oggetto
        // più l'angolo diminuisce, più il coseno aumenta
        // se il coseno è inferiore vuol dire che l'angolo è più ampio rispetto a quello limite, quindi fuori -> non lo vede
        if (Vector3.Dot(transform.forward, toTarget) < Mathf.Cos(_viewAngle / 2 * Mathf.Deg2Rad)) return false;

        // se ci sono ostacoli -> non lo vede
        if (Physics.Raycast(_head.position, toTarget, _sightDistance, _obstacle)) return false;

        return true;
    }

    private void DrawVisionCone()
    {
        float startAngle = -_viewAngle / 2;

        Vector3 origin = _head.position;
        Vector3 forward = transform.forward;

        _lineRenderer.SetPosition(0, origin);

        for (int i = 0; i <= _segments; i++)
        {
            float currentAngle = startAngle + (_viewAngle / _segments) * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
            Vector3 point = origin + direction * _radius;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, _radius, _obstacle))
            {
                point = hit.point;
            }

            _lineRenderer.SetPosition(i + 1, point);
        }
    }
}
