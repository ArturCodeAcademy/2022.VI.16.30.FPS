using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private const float MAX_LASER_LENGTH = 10000;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        _lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _lineRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        _lineRenderer.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, MAX_LASER_LENGTH))
            _lineRenderer.SetPosition(1, hit.point);
        else
            _lineRenderer.SetPosition(1, transform.position + transform.forward * MAX_LASER_LENGTH);
    }
}
