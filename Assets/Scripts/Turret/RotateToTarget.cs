using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    [field:SerializeField] public Transform Target { get; set; }
    [field: SerializeField] public float VerticalOffset { get; set; }

    [SerializeField, Range(-180, 0)] private float _minVerticalAngle;
    [SerializeField, Range(0, 180)] private float _maxVerticalAngle;
    [SerializeField, Range(0.1f, 720f)] private float _speed;

    private void LateUpdate()
    {
        if (Target == null)
            return;

        Vector3 targetDir = Target.position - transform.position + Vector3.up * VerticalOffset;

        Vector3 euler = Quaternion.RotateTowards
        (
            transform.rotation,
            Quaternion.LookRotation(targetDir),
            Time.deltaTime * _speed
        ).eulerAngles;
        if (euler.x > 180)
            euler.x -= 360; 
        euler.x = Mathf.Clamp(euler.x, _minVerticalAngle, _maxVerticalAngle);
        euler.z = transform.rotation.z;

        transform.rotation = Quaternion.Euler(euler);
    }
}
