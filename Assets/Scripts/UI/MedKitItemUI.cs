using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(MedKitItem))]
public class MedKitItemUI : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;

    private MedKitItem _medKit;

    private void OnEnable()
    {
        _medKit ??= GetComponent<MedKitItem>();
        _medKit.OnHealthValueChanged += UpdateUI;
    }

    private void UpdateUI((float currect, float max) health)
    {
        if (_healthSlider != null)
        {
            _healthSlider.maxValue = health.max;
            _healthSlider.value = health.currect;
        }
    }

    private void OnDisable()
    {
        _medKit.OnHealthValueChanged -= UpdateUI;
    }
}
