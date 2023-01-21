using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerFallJumpDuration : MonoBehaviour
{
    [SerializeField, Min(1)] private float _fallMultiplier = 1;
    [SerializeField, Min(1)] private float _lowJumpMultiplier = 1;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.Space))
            _rigidbody.velocity += Physics.gravity * Time.deltaTime
                * ((_rigidbody.velocity.y < 0? _fallMultiplier : _lowJumpMultiplier) - 1);
    }
}
