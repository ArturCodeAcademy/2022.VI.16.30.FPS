using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaDamager : MonoBehaviour
{
    [SerializeField, Min(0)] private float _damagePerSecond;
    private HashSet<Health> _healths = new HashSet<Health>();

    void Update()
    {
        foreach (Health health in _healths)
            health.Hit(_damagePerSecond * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health))
            _healths.Add(health);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Health health)
            && _healths.Contains(health))
            _healths.Remove(health);
    }
}
