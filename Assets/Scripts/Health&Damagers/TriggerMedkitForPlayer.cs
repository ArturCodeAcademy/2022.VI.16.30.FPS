using UnityEngine;

public class TriggerMedkitForPlayer : MonoBehaviour
{
    public float LeftHealth => _health;
    public UnityEventMedkit OnMedkitUsed;

    [SerializeField, Min(1)] private float _health;

    private void Awake()
    {
        OnMedkitUsed ??= new UnityEventMedkit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == Player.Instance.transform
            && other.TryGetComponent(out Health health))
        {
            float taken = health.AddHealth(_health);
            _health -= taken;
            OnMedkitUsed?.Invoke(new()
            {
                HealthTaken = taken,
                LeftHealth = _health
            });

            if (_health <= 0)
                Destroy(gameObject);
        }
    }
}
