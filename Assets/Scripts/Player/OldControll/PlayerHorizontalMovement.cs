using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundedCheck), typeof(PlayerCrouching), typeof(Rigidbody))]
public class PlayerHorizontalMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField, Min(0)] private float _walkSpeed;
    [SerializeField, Min(0)] private float _runSpeed;
    [SerializeField, Min(0)] private float _speedInAir;
    [SerializeField, Min(0)] private float _crouchSpeed;

    private GroundedCheck _groundedCheck;
    private PlayerCrouching _crouching;
    private Rigidbody _rigidbody;

    private Vector3 _impulse;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _crouching = GetComponent<PlayerCrouching>();
        _groundedCheck = GetComponent<GroundedCheck>();
    }

    private void Update()
    {
        _impulse = new Vector3
        (
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        ).normalized;

        if (_groundedCheck.IsGrounded)
        {
            if (_crouching.IsCrouched)
                _impulse *= _crouchSpeed;
            else
                _impulse *= Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;
        }
        else
            _impulse *= _speedInAir;
    }

    private void FixedUpdate() => _rigidbody.AddRelativeForce(_impulse, ForceMode.Impulse);

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (_walkSpeed > _runSpeed)
            _walkSpeed = _runSpeed;

        if (_crouchSpeed > _walkSpeed)
            _crouchSpeed = _walkSpeed;
    }

#endif
}
