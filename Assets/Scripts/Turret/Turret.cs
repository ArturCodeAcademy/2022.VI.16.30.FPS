using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private StealthForPlayer _stealthForPlayer;
    [SerializeField] private Quaternion _defaultRotation;
    [SerializeField, Range(0, 50)] private float _rotationSpeed;

    private RotateToTarget _rotateToTarget;
    private Laser _laser;
    private Coroutine _coroutine;

    void Start()
    {
        _rotateToTarget = GetComponentInChildren<RotateToTarget>();
        _laser = GetComponentInChildren<Laser>();

        _rotateToTarget.enabled = false;
        _laser.enabled = false;

        _stealthForPlayer.OnReact.AddListener(StartRotation);
        _stealthForPlayer.OnCalmDown.AddListener(SetDefaultRotation);
        _coroutine = StartCoroutine(RotateToDefaultState());
    }

    void OnDisable()
    {
        _stealthForPlayer.OnReact.RemoveListener(StartRotation);
        _stealthForPlayer.OnCalmDown.RemoveListener(SetDefaultRotation);
    }

    void StartRotation(StealthEventArgs args)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _rotateToTarget.enabled = true;
        _rotateToTarget.Target = args.Target;
        _laser.enabled = true;
    }

    void SetDefaultRotation(StealthEventArgs args)
    {
        _rotateToTarget.enabled = false;
        _laser.enabled = false;
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(RotateToDefaultState());
    }

    IEnumerator RotateToDefaultState()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (_rotateToTarget.transform.localRotation != _defaultRotation)
        {
            _rotateToTarget.transform.localRotation = Quaternion.RotateTowards(_rotateToTarget.transform.localRotation,
                _defaultRotation, _rotationSpeed * Time.deltaTime);
            yield return wait;
        }
    }
}
