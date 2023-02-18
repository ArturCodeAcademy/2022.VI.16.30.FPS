using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunBase : MonoBehaviour, IGun
{
    public Action OnShoot;

    [field: SerializeField] public Vector3 NormalPosition { get; private set; }
    [field: SerializeField] public Quaternion NormalRotation { get; private set; }

    [field: SerializeField] public Vector3 AimingPosition { get; private set; }
    [field: SerializeField] public Quaternion AimingRotation { get; private set; }

    [field: SerializeField, Min(0.05f)] public float AimingTransitionDuration { get; private set; }
    [field: SerializeField] public AnimationCurve AimingTransitionCurve { get; private set; }

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
	[field: SerializeField] public int AmmoCountInGun { get; private set; }
	[field: SerializeField] public AmmoTypes.Type AmmoType { get; private set; }
	// New
	[Header("Effects")] 
    [SerializeField] protected ParticleEffectsPool _hitEffectsPool;
    [SerializeField] protected ParticleEffectsPool _shootEffectsPool;
    [SerializeField] protected SpritesPool _holesPool;
    // End new
    private Transform _playerCamera;

    private void Start()
    {
        _playerCamera = Player.Instance.Camera.transform;
    }

    protected void Shoot(float damage, float bulletSpread, float penetratingPower, float impulse)
    {
        if (AmmoCountInGun <= 0)
            return;

        AmmoCountInGun--;
        OnShoot?.Invoke();
        ParticlePoolElement shootEffect = _shootEffectsPool.GetElement();
        shootEffect.transform.position = _muzzle.position;
        shootEffect.transform.rotation = _muzzle.rotation;

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
