using TMPro;
using UnityEngine;

public class MedkitUI : MonoBehaviour
{
    [SerializeField] private TriggerMedkitForPlayer _medkit;
    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        _medkit.OnMedkitUsed.AddListener(UpdateUI);
    }

    private void UpdateUI(MedkitEventArgs args)
    {
        _text.text = $"{(int)args.LeftHealth}";
    }

    private void OnDisable()
    {
        _medkit.OnMedkitUsed.RemoveListener(UpdateUI);
    }
}
