using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class CharacterGravity : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public UnityEventOnGetGrounded OnGetGrounded;

    [SerializeField] private float _gravityScale = 1;
    [SerializeField] private float _groundedTimer = 0.2f;

    private CharacterController _characterController;
    private float _verticalVelocity = 0;
    private float _timer = 0;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        OnGetGrounded ??= new UnityEventOnGetGrounded();
    }

    private void LateUpdate()
    {
        _verticalVelocity -= Physics.gravity.magnitude * Time.deltaTime * _gravityScale;
        CollisionFlags collisionFlags = _characterController.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
        if (collisionFlags == CollisionFlags.CollidedBelow)
        {
            IsGrounded = true;
            _timer = _groundedTimer;
            OnGetGrounded.Invoke(new GroundedEventArgs() { VerticalVelocity = _verticalVelocity });
            _verticalVelocity = 0;
        }
        else
        {
            if (_timer > 0)
                _timer -= Time.deltaTime;
            else
                IsGrounded = false;
        }
    }

    public void SetVelocity(float velocity)
    {
        _verticalVelocity = velocity;
    }
}
