using UnityEngine;

public class FireZoneController : MonoBehaviour
{
    [SerializeField] private bool _enableOnAwake;
    [SerializeField] private ParticleSystem _fireEffect;
    [SerializeField] private TriggerAreaDamager _damager;

    private void Awake()
    {
        if (_enableOnAwake)
            TurnOn();
        else
            TurnOff();
    }

    public void TurnOn()
    {
        _fireEffect.Play();
        _damager.enabled = true;
    }

    public void TurnOff()
    {
        _fireEffect.Stop();
        _damager.enabled = false;
    }
}
