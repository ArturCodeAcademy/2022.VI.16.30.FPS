using System;
using UnityEngine;

public class MedKitItem : Item
{
    public event Action<(float current, float max)> OnHealthValueChanged;

    [SerializeField, Min(1)] private float _maxHealth;
    [SerializeField, Min(1)] private float _beginHealth;
    [SerializeField, Range(0.1f, 10)] private float _healthHealSpeed;
    [SerializeField, Range(0.1f, 10)] private float _healthRecoverSpeed;

    private float _currentHealth;
    private Health _playerHealth;

    private void Start()
    {
        _currentHealth = _beginHealth;
        _playerHealth = Player.Instance.GetComponent<Health>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float h = Mathf.Min(_currentHealth, _healthHealSpeed * Time.deltaTime);
            _currentHealth -= h;
            _playerHealth.AddHealth(h);
            OnHealthValueChanged?.Invoke((_currentHealth, _maxHealth));
        }
        else if (_currentHealth < _maxHealth)
        {
            float h = Mathf.Min(_maxHealth - _currentHealth, _healthRecoverSpeed * Time.deltaTime);
            _currentHealth += h;
            OnHealthValueChanged?.Invoke((_currentHealth, _maxHealth));
        }
    }
}
