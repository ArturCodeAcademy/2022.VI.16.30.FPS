using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(AudioSource))]
public class TurretShoot : MonoBehaviour
{
    [Header("Params")]
    [SerializeField, Min(0)] private float _damage;
    [SerializeField, Min(0)] private float _shootDistance;
    [SerializeField, Min(0)] private float _aimingDuration;
    [SerializeField, Min(0)] private float _cooldownDuration;
    [SerializeField] private Gradient _aimingGradient;
    
    [Header("Audio")]
    [SerializeField] private AudioClip _aimingClip;
    [SerializeField] private AudioClip _shootClip;
    
    [Header("Behaviours")]    
    [SerializeField] private StealthForPlayer _stealthForPlayer;

    [Header("Objects / Prefabs")]
    [SerializeField] private GameObject _shootEffect;
    [SerializeField, Min(0)] private float _effectDuration;
    [SerializeField] private Transform[] _guns;
    
    private LineRenderer _laser;
    private AudioSource _audioSource;
    
    private Coroutine _coroutine;

    void Start()
    {
        _laser = GetComponent<LineRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _stealthForPlayer?.OnReact?.AddListener(StartShooting);
        _stealthForPlayer?.OnCalmDown?.AddListener(StopShooting);
    }

    void OnEnable()
    {
        _stealthForPlayer?.OnReact?.AddListener(StartShooting);
        _stealthForPlayer?.OnCalmDown?.AddListener(StopShooting);
    }

    private void OnDisable()
    {
        _stealthForPlayer?.OnReact.RemoveListener(StartShooting);
        _stealthForPlayer?.OnCalmDown.RemoveListener(StopShooting);
    }

    void StartShooting(StealthEventArgs args)
    {
        StopShooting(args);
        _coroutine = StartCoroutine(Shooting(args.Target));
    }

    void StopShooting(StealthEventArgs args)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    IEnumerator Shooting(Transform target)
    {
        WaitForSeconds waitCooldown = new WaitForSeconds(_cooldownDuration);
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        Health health = target.GetComponent<Health>();

        while (true)
        {
            float time = 0;
            do
            {
                Physics.Raycast(_laser.transform.position, _laser.transform.forward, out RaycastHit hit, _shootDistance);
                time += hit.transform == target? Time.deltaTime : -Time.deltaTime;
                time = Mathf.Max(time, 0);
                if (_aimingDuration - time <= _aimingClip.length && !_audioSource.isPlaying)
                    _audioSource.PlayOneShot(_aimingClip);
                if (_aimingDuration - time > _aimingClip.length)
                    _audioSource.Stop();
                _laser.startColor = _aimingGradient.Evaluate(time / _aimingDuration);
                _laser.endColor = _aimingGradient.Evaluate(time / _aimingDuration);

                yield return waitFrame;
            }
            while (time < _aimingDuration);

            health.Hit(_damage);
            _audioSource.PlayOneShot(_shootClip);
            _laser.startColor = _aimingGradient.Evaluate(0);
            _laser.endColor = _aimingGradient.Evaluate(0);
            foreach (var gun in _guns)
                Destroy(Instantiate(_shootEffect, gun.position, gun.rotation), _effectDuration);

            yield return waitCooldown;
        }
    }
}
