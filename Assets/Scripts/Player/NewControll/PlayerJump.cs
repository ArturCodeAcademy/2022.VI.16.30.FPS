using UnityEngine;

namespace NewControll
{
    [RequireComponent(typeof(CharacterGravity), typeof(CharacterController))]
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _jumpHeight;
        [SerializeField, Min(0)] private int _maxJumpsCount;
        
        private CharacterController _characterController;
        private CharacterGravity _gravity;
        private float _jumpForce;
        private int _leftJumpsCount;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _gravity = GetComponent<CharacterGravity>();
            _jumpForce = CalculateJumpForce();
            RefreshLeftJumpsCount();
            _gravity.OnGetGrounded.AddListener(RefreshLeftJumpsCount);
        }

        private void Update()
        {
            if (_leftJumpsCount > 0 && Input.GetKeyDown(KeyCode.Space))
            {
                _leftJumpsCount--;
                _gravity.SetVelocity(_jumpForce);
            }
        }

        private void OnDisable()
        {
            _gravity.OnGetGrounded.RemoveListener(RefreshLeftJumpsCount);
        }

        private void RefreshLeftJumpsCount(GroundedEventArgs _ = default)
            => _leftJumpsCount = _maxJumpsCount;

        private float CalculateJumpForce()
            => Mathf.Sqrt(2 * _jumpHeight * Physics.gravity.magnitude);
    }
}