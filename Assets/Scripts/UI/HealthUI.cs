using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Image _sliderFill;
    [SerializeField] private Gradient _fillGradient;
    private Health _health;

    private void Start()
    {
        _health = GetComponent<Health>();
        _health.OnHealthValueChanged.AddListener(UpdateUI);
        UpdateUI(new()
        {
            CurrentHealth = _health.CurrentHealth,
            MaxHealth = _health.MaxHealth
        });
    }

    private void UpdateUI(HealthEventArgs args)
    {
        if (_healthText != null)
            _healthText.text = $"{(int)args.CurrentHealth} / {(int)args.MaxHealth}";
        if (_healthSlider != null)
        {
            _healthSlider.maxValue = args.MaxHealth;
            _healthSlider.value = args.CurrentHealth;
        }
        if (_sliderFill != null)
            _sliderFill.color = _fillGradient.Evaluate(args.CurrentHealth / args.MaxHealth);
    }

    private void OnDisable()
    {
        _health?.OnHealthValueChanged.RemoveListener(UpdateUI);
    }
}
