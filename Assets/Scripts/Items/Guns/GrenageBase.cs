using System;
using UnityEngine;

public class GrenageBase : Item, IGun, INotThrowable, ISwitchSkipable
{
    public event Action OnShoot;
    public event Action<float> OnStartReloading;
    public event Action OnDropReloading;
    public event Action OnReloaded;

    public Vector3 AimingPosition => NormalPosition;
    public Quaternion AimingRotation => NormalRotation;
    public float AimingTransitionDuration => 0;
    public AnimationCurve AimingTransitionCurve => AnimationCurve.Linear(0, 0, 1, 1);
    public int AmmoCountInGun { get; private set; }
    [field:SerializeField] public AmmoTypes.Type AmmoType { get; private set; }

    [SerializeField, Min(0)] private float _throwImpulse;
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField, Min(0.1f)] protected float _reloadingDuration;

    private GameObject _grenadeObject;
    private AmmoBackpack _ammoBackpack;
    protected float _reloadingTimeElapsed;

    private const int MAGAZINE_VOLUME = 1;

    protected virtual void Start()
    {
        _ammoBackpack = Player.Instance.GetComponent<AmmoBackpack>();
    }

    protected virtual void Update()
    {
        Throw();
        TakeNewGrenade();
    }

    private void Throw()
    {
        if (AmmoCountInGun <= 0)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _grenadeObject.transform.SetParent(null);
            _grenadeObject.GetComponentInChildren<Collider>().enabled = true;
            _grenadeObject.GetComponentInChildren<Grenade>().enabled = true;
            _grenadeObject.GetComponentInChildren<TrailRenderer>().enabled = true;
            Rigidbody rigidbody = _grenadeObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rigidbody.AddForce(Camera.main.transform.forward * _throwImpulse, ForceMode.Impulse);
            AmmoCountInGun--;
            OnShoot?.Invoke();
        }    
    }

    private void TakeNewGrenade()
    {
        if (MAGAZINE_VOLUME == AmmoCountInGun || _ammoBackpack.GetAmmoCount(AmmoType) == 0)
            return;

        if (Input.GetKeyUp(KeyCode.R) && _reloadingTimeElapsed != 0)
        {
            _reloadingTimeElapsed = 0;
            OnDropReloading?.Invoke();
            return;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            _reloadingTimeElapsed += Time.deltaTime;
            OnStartReloading?.Invoke(_reloadingDuration);
            return;
        }
        else if (Input.GetKey(KeyCode.R) && _reloadingTimeElapsed != 0)
        {
            _reloadingTimeElapsed += Time.deltaTime;
            if (_reloadingTimeElapsed >= _reloadingDuration)
            {
                AmmoCountInGun = _ammoBackpack.GetAmmo(AmmoType, MAGAZINE_VOLUME);
                if (AmmoCountInGun != 0)
                {
                    _grenadeObject = Instantiate(_grenadePrefab, transform);
                    _grenadeObject.transform.localPosition = Vector3.zero;
                    _grenadeObject.transform.localRotation = Quaternion.identity;
                    _grenadeObject.GetComponentInChildren<Collider>().enabled = false;
                }

                _reloadingTimeElapsed = 0;
                OnReloaded?.Invoke();
            }
        }
    }

    public bool Skip()
    {
        _ammoBackpack ??= Player.Instance.GetComponent<AmmoBackpack>();
        return _ammoBackpack.GetAmmoCount(AmmoType) <= 0 && AmmoCountInGun <= 0;
    }
}
