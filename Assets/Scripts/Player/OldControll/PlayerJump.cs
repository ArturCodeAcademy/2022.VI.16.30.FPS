using UnityEngine;

[RequireComponent(typeof(GroundedCheck), typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField, Min(0)] private float _jumpHeight;
    [SerializeField, Min(0)] private int _maxJumpsCount;

    private Rigidbody _rigidbody;
    private GroundedCheck _groundedCheck;
    private float _jumpForce;
    private int _leftJumpsCount;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundedCheck = GetComponent<GroundedCheck>();
        _jumpForce = CalculateJumpForce();
        RefreshLeftJumpsCount();
        _groundedCheck.OnGetGrounded += RefreshLeftJumpsCount;
    }

    private void Update()
    {
        if (_leftJumpsCount > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            _leftJumpsCount--;

            Vector3 velocity = _rigidbody.velocity;
            velocity.y = 0.0f;
            _rigidbody.velocity = velocity;

            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void OnDisable()
    {
        _groundedCheck.OnGetGrounded -= RefreshLeftJumpsCount;
    }

    private void RefreshLeftJumpsCount() => _leftJumpsCount = _maxJumpsCount;

    private float CalculateJumpForce()
        => _rigidbody.mass * Mathf.Sqrt(2 * _jumpHeight * Physics.gravity.magnitude);
}
