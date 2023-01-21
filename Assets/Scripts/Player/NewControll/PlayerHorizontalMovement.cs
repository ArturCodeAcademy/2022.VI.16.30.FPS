using UnityEngine;

namespace NewControll
{
    [RequireComponent(typeof(CharacterGravity), typeof(PlayerCrouching), typeof(CharacterController))]
    public class PlayerHorizontalMovement : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField, Min(0)] private float _walkSpeed;
        [SerializeField, Min(0)] private float _runSpeed;
        [SerializeField, Min(0)] private float _speedInAir;
        [SerializeField, Min(0)] private float _crouchSpeed;

        private CharacterGravity _gravity;
        private PlayerCrouching _crouching;
        private CharacterController _controller;

        private void Awake()
        {
            _gravity = GetComponent<CharacterGravity>();
            _crouching = GetComponent<PlayerCrouching>();
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Vector3 movemet = new Vector3
            (
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            ).normalized;
            movemet = transform.TransformDirection(movemet);

            if (_gravity.IsGrounded)
            {
                if (_crouching.IsCrouched)
                    movemet *= _crouchSpeed;
                else
                    movemet *= Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;
            }
            else
                movemet *= _speedInAir;

            _controller.Move(movemet * Time.deltaTime);
        }

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
}