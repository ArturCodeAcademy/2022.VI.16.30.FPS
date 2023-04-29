using UnityEngine;

public class FlashLight : Item
{
    [SerializeField] private AudioClip _swichSound;

    private Light _light;
    private bool _isOn = false;

    protected override void Awake()
    {
        base.Awake();
        _light = GetComponentInChildren<Light>();
        _light.enabled = _isOn;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isOn = !_isOn;
            _light.enabled = _isOn;
            AudioSource.PlayClipAtPoint(_swichSound, transform.position);
        }
    }

    public override void Drop()
    {
        base.Drop();
        _light.enabled = false;
    }
}
