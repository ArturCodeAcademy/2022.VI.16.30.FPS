using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(GroundedCheck))]
public class HorizontalDrag : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _dragFactor = 0.7f;
    [SerializeField, Range(0, 1)] private float _dragFactorInAir = 0.03f;

    private Rigidbody _rigidbody;
    private GroundedCheck _groundedCheck;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundedCheck = GetComponent<GroundedCheck>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _rigidbody.velocity;
        velocity.x *= 1.0f - (_groundedCheck.IsGrounded ? _dragFactor : _dragFactorInAir);
        velocity.z *= 1.0f - (_groundedCheck.IsGrounded ? _dragFactor : _dragFactorInAir);
        _rigidbody.velocity = velocity;
    }
}
