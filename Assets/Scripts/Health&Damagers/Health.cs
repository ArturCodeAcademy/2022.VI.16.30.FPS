using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IHitable
{
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    public UnityEvent OnHealthEnd;
    public UnityEventHealth OnHealthValueChanged;

    [SerializeField, Min(0)] private float _maxHealth;
    private float _currentHealth;

    private void Awake()
    {
        OnHealthEnd ??= new UnityEvent();
        OnHealthValueChanged ??= new UnityEventHealth();
        _currentHealth = _maxHealth;
    }

    public float Hit(float damage)
    {
        if (damage <= 0)
            return 0;

        float res = Mathf.Min(damage, _currentHealth);
        _currentHealth -= res;
        OnHealthValueChanged?.Invoke(new()
        {
            CurrentHealth = _currentHealth,
            MaxHealth = _maxHealth
        });

        if (_currentHealth <= 0)
            OnHealthEnd?.Invoke();

        return res;
    }

    public float AddHealth(float health)
    {
        if (health <= 0)
            return 0;

        float res = Mathf.Min(health, _maxHealth - _currentHealth);
        _currentHealth += res;
        OnHealthValueChanged?.Invoke(new()
        {
            CurrentHealth = _currentHealth,
            MaxHealth = _maxHealth
        });
        return res;
    }

    public void Kill()
    {
        Hit(_currentHealth);
    }
}
