using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class OnTurretEndHealth : MonoBehaviour
{
    [SerializeField, Min(0)] private float _destroyPause;
    [SerializeField, Min(0)] private float _destroyForce;
    [SerializeField] private AudioClip _destroySound;

    private const float DESTROY_RANGE = 20;
    private static ParticleEffectsPool _explosionEffects;

    private Health _health;

    private void Start()
    {
        _health = GetComponent<Health>();
        _health.OnHealthEnd.AddListener(OnEndHealth);
        _explosionEffects ??= FindObjectsOfType<ParticleEffectsPool>().First(x => x.Name.Equals("Explosion"));
    }

    private void OnEndHealth()
    {
        MonoBehaviour[] behaviours =
        {
            GetComponent<Turret>(),
            GetComponentInChildren<Laser>(),
            GetComponentInChildren<RotateToTarget>(),
            GetComponentInChildren<ShootTarget>(),
            GetComponent<TurretForTarget>(),
            GetComponentInChildren<TurretShoot>()
        };
        foreach (var behaviour in behaviours)
            if (behaviour != null)
                try
                {
                    Destroy(behaviour);
                }
                finally {}
        List<Transform> parts = new List<Transform>();  
        Detach(transform, parts);
        foreach (var part in parts)
        {
            part.parent = null;
            Destroy(part.gameObject, _destroyPause);
        }
        ParticlePoolElement shootEffect = _explosionEffects.GetElement();
        shootEffect.transform.position = transform.position;
        shootEffect.transform.rotation = Quaternion.identity;
        AudioSource.PlayClipAtPoint(_destroySound, transform.position);
        Destroy(gameObject);
    }

    private void Detach(Transform parent, List<Transform> parts)
    {
        foreach (Transform child in parent)
            Detach(child, parts);
        if (parent.TryGetComponent(out Collider _))
        {
            Rigidbody rb = parent.gameObject.AddComponent<Rigidbody>();
            rb.AddExplosionForce(_destroyForce, transform.position, DESTROY_RANGE);
            parts.Add(parent);
        }     
    }

    private void OnDestroy()
    {
        if (_health != null)
            _health.OnHealthEnd.RemoveListener(OnEndHealth);
    }
}
