using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunBase : Item, IGun
{
    public event Action OnShoot;
    public event Action<float> OnStartReloading;
    public event Action OnDropReloading;
    public event Action OnReloaded;

    [field: SerializeField] public Vector3 AimingPosition { get; protected set; }
    [field: SerializeField] public Quaternion AimingRotation { get; protected set; }

    [field: SerializeField, Min(0.05f)] public float AimingTransitionDuration { get; protected set; }
    [field: SerializeField] public AnimationCurve AimingTransitionCurve { get; protected set; }

    [Header("Elements")]
    [SerializeField] protected Transform _muzzle;

    [Header("Params")]
    [SerializeField, Range(0, 45)] protected float _normalSpread;
    [SerializeField, Range(0, 45)] protected float _aimSpread;
    [Space(10)]
    [SerializeField, Min(0)] protected float _fireRatePerSecond;
    [SerializeField, Min(0)] protected float _damage;
    [SerializeField, Min(0)] protected float _penetratingPower;
    [SerializeField, Min(0)] protected float _impulsePower;
    [Space(10)]
    [SerializeField, Min(1)] protected int _magazineVolume;
	[field: SerializeField] public int AmmoCountInGun { get; protected set; }
	[field: SerializeField] public AmmoTypes.Type AmmoType { get; protected set; }
	
    [Header("Effects")] 
    [SerializeField] protected ParticleEffectsPool _hitEffectsPool;
    [SerializeField] protected ParticleEffectsPool _shootEffectsPool;
    [SerializeField] protected SpritesPool _holesPool;

    [Header("Reloading")]
    [SerializeField, Min(0.1f)] protected float _reloadingDuration;

    protected float _reloadingTimeElapsed;
    protected float _cooldownPause;
    protected AmmoBackpack _ammoBackpack;

    private Transform _playerCamera;

	protected virtual void Start()
    {
        _ammoBackpack = Player.Instance.GetComponent<AmmoBackpack>();
        _playerCamera = Player.Instance.Camera.transform;
	}

	protected virtual void Update()
	{
		Reload();
        if (_cooldownPause > 0)
            _cooldownPause -= Time.deltaTime;
	}

	protected virtual void Reload()
	{
        if (_magazineVolume == AmmoCountInGun || _ammoBackpack.GetAmmoCount(AmmoType) == 0)
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
				FillMagazine();
				_reloadingTimeElapsed = 0;
			}
		}
	}

    protected virtual void FillMagazine()
    {
        int need = _magazineVolume - AmmoCountInGun;
        int got = _ammoBackpack.GetAmmo(AmmoType, need);
        AmmoCountInGun += got;
        OnReloaded?.Invoke();
	}

    protected void InvokeOnReloaded() => OnReloaded?.Invoke(); 

	protected void Shoot(float damage, float bulletSpread, float penetratingPower, float impulse, int bulletCount = 1)
    {
        if (AmmoCountInGun <= 0 || _cooldownPause > 0)
            return;

        _cooldownPause = _fireRatePerSecond;
		AmmoCountInGun--;
        OnShoot?.Invoke();
        ParticlePoolElement shootEffect = _shootEffectsPool.GetElement();
        shootEffect.transform.position = _muzzle.position;
        shootEffect.transform.rotation = _muzzle.rotation;

        for (int i = 0; i < bulletCount; i++)
        { 
            IEnumerable<RaycastHit> hits = GetHits(bulletSpread);
            if (hits == null || hits.Count() == 0)
                return;

            Vector3 direction = hits.First().point - _playerCamera.position;
            float leftPenetratingPower = penetratingPower;
            foreach(RaycastHit hit in hits)
            {
                Debug.DrawRay(_playerCamera.position, direction, Color.green, 30);
                Debug.DrawLine(_playerCamera.position, hit.point, Color.red, 30);

                if (hit.transform.TryGetComponent(out IHitable hitable))           
                    hitable.Hit(damage * leftPenetratingPower / penetratingPower);

                if (hit.transform.TryGetComponent(out Rigidbody rigidbody))
                    rigidbody.AddForce(impulse * leftPenetratingPower / penetratingPower * direction);

                ParticlePoolElement hitEffect = _hitEffectsPool.GetElement();
                hitEffect.transform.position = hit.point;
                hitEffect.transform.forward = hit.normal;

                SpritePoolElement hole = _holesPool.GetElement();
                hole.transform.position = hit.point;
                hole.transform.forward = hit.normal;

                if (hit.transform.TryGetComponent(out BulletBarrier barrier))
                {
                    leftPenetratingPower -= barrier.Hardness;
                    if (leftPenetratingPower <= 0)
                        break;
                }
                else
                    break;
            }
		}
	}

    protected IEnumerable<RaycastHit> GetHits(float bulletSpread = 0)
    {
        Vector3 direction = GetDirectionWithSpread(bulletSpread, _playerCamera.forward);
        IEnumerable<RaycastHit>  hits = Physics.RaycastAll(_playerCamera.position, direction);
        // New
        return hits?.Where(x => x.transform != Player.Instance.transform)
            .OrderBy(x => Vector3.Distance(_playerCamera.position, x.point));
    }

    private Vector3 GetDirectionWithSpread(float bulletSpread, Vector3 forward)
    {
        Vector3 angle = UnityEngine.Random.onUnitSphere;
        angle.z = 0;
        angle.Normalize();
        angle *= UnityEngine.Random.Range(0, bulletSpread);
        return Quaternion.Euler(angle) * forward;
    }
}
