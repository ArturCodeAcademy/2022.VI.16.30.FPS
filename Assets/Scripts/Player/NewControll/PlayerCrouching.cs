using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewControll
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCrouching : MonoBehaviour
    {
        [SerializeField, Min(0.01f)] private float _crouchStep;
        [SerializeField, Min(0)] private float _crouchHeight;

        private CharacterController _capsuleCollider;

        public bool IsCrouched { get; private set; }

        private float _playerHeight;
        private float _cameraOffsetY;
        private Coroutine _coroutine;

        private void Awake()
        {
            _capsuleCollider = GetComponent<CharacterController>();
            _playerHeight = _capsuleCollider.height;
            _cameraOffsetY = _playerHeight - Camera.main.transform.localPosition.y;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(Sit(true));
            }
            else if (_capsuleCollider.height < _playerHeight)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(Sit(false));
            }
        }

        private bool CanMoveUp()
        {
            const float RAY_OFFSET_Y = -0.2f;
            const float RAY_DISTANCE = 0.3f;
            Vector3 rayStartPos = transform.position + Vector3.up * (_capsuleCollider.height + RAY_OFFSET_Y);
            return !Physics.Raycast(rayStartPos, Vector3.up, RAY_DISTANCE);
        }

        private IEnumerator Sit(bool moveDown)
        {
            IsCrouched = true;
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            while (moveDown ? _capsuleCollider.height > _crouchHeight : _capsuleCollider.height < _playerHeight)
            {
                if (!moveDown && !CanMoveUp())
                {
                    yield return wait;
                    continue;
                }
                _capsuleCollider.height += (moveDown ? -1 : 1) * _crouchStep * Time.deltaTime;
                _capsuleCollider.center = Vector3.up / 2 * _capsuleCollider.height;
                Camera.main.transform.localPosition = (_capsuleCollider.height - _cameraOffsetY) * Vector3.up;
                yield return wait;
            }

            _capsuleCollider.height = moveDown ? _crouchHeight : _playerHeight;
            _capsuleCollider.center = Vector3.up / 2 * _capsuleCollider.height;
            Camera.main.transform.localPosition = (_capsuleCollider.height - _cameraOffsetY) * Vector3.up;
            IsCrouched = moveDown;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            _capsuleCollider ??= GetComponent<CharacterController>();
            if (_capsuleCollider != null && _crouchHeight > _capsuleCollider.height)
                _crouchHeight = _capsuleCollider.height;
        }

#endif
    }
}
