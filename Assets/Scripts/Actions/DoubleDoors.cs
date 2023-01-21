using UnityEngine;

public class DoubleDoors : MonoBehaviour
{
    [SerializeField] private bool _isOpened;
    [SerializeField, Range(0, 180)] private float _speed;

    [Header("Left door")]
    [SerializeField] private Transform _leftDoor;   
    [SerializeField] private Quaternion _leftDoorOpened;
    [SerializeField] private Quaternion _leftDoorClosed;
    [Header("Right door")]
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Quaternion _rightDoorOpened;
    [SerializeField] private Quaternion _rightDoorClosed;

    private Quaternion _leftDoorTarget;
    private Quaternion _rightDoorTarget;

    private void Start()
    {
        if (_isOpened)
            OpenDoor();
        else
            CloseDoor();
    }

    private void Update()
    {
        if (_leftDoorTarget == _leftDoor.rotation
            && _rightDoorTarget == _rightDoor.rotation)
            return;

        _leftDoor.localRotation = Quaternion.RotateTowards(_leftDoor.localRotation,
            _leftDoorTarget, _speed * Time.deltaTime);
        _rightDoor.localRotation = Quaternion.RotateTowards(_rightDoor.localRotation,
            _rightDoorTarget, _speed * Time.deltaTime);
    }

    public void OpenDoor()
    {
        _leftDoorTarget = _leftDoorOpened;
        _rightDoorTarget = _rightDoorOpened;
    }

    public void CloseDoor()
    {
        _leftDoorTarget = _leftDoorClosed;
        _rightDoorTarget = _rightDoorClosed;
    }
}
