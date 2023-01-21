using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField, Range(30, 179)] private float _verticalAngle;
    [SerializeField, Min(0)] private float _turnSpeed = 0.2f;
    [SerializeField] private bool _verticalInvertion;
    [SerializeField] private Camera _playerCamera;

    private float _xAxis = 0f;

    private void Update()
    {
       if (Time.timeScale == 0)
            return;

        Vector2 rawLookVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 lookVector = rawLookVector * _turnSpeed;

        _xAxis = Mathf.Clamp(_xAxis + (_verticalInvertion ? lookVector.y : -lookVector.y),
            -_verticalAngle / 2, _verticalAngle / 2);
        transform.Rotate(Vector3.up * lookVector.x);

        _playerCamera.transform.localRotation = Quaternion.Euler(Vector3.right * _xAxis);
    }
}
