using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunBase : MonoBehaviour, IGun
{
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

    private Transform _playerCamera;

    private void Start()
    {
        _playerCamera = Player.Instance.Camera.transform;
    }

    protected void Shoot(float damage, float bulletSpread, float penetratingPower, float impulse)
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

            // TODO: Add holes and effects

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
        return hits?.OrderBy(x => Vector3.Distance(_playerCamera.position, x.point));
    }

    private Vector3 GetDirectionWithSpread(float bulletSpread, Vector3 forward)
    {
        Vector3 angle = Random.onUnitSphere;
        angle.z = 0;
        angle.Normalize();
        angle *= Random.Range(0, bulletSpread);
        return Quaternion.Euler(angle) * forward;
    }
}
