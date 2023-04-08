using UnityEngine;

public class AmmoInteractable : MonoBehaviour, IInteractable
{
    public string Info => $"{_type}: {_ammoCount}({_gunName})";

    [SerializeField] private string _gunName = string.Empty;
    [SerializeField, Min(1)] private int _ammoCount;
    [SerializeField] private AmmoTypes.Type _type;

    private AmmoBackpack _backpack;

    public void Interact()
    {
        _backpack ??= Player.Instance.GetComponent<AmmoBackpack>();
        _backpack.AddAmmo(_type, _ammoCount);
        Destroy(gameObject);
    }
}
