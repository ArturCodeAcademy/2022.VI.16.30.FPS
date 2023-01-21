using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class ChangeCameraState : MonoBehaviour
{
    private Scanner _scanner;
    private bool _isOn = true;

    private void Start()
    {
        _scanner = GetComponent<Scanner>();
    }

    public void ChangeState()
    {
        _isOn = !_isOn;
        _scanner.enabled = _isOn;
    }
}
