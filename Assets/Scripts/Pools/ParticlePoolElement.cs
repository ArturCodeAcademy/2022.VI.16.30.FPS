using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePoolElement : PoolElement
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public override void OnGetElement()
    {
        gameObject.SetActive(true);
        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        Release();
    }
}
