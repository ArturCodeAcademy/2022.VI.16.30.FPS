using UnityEngine;

public class Smoke : Grenade
{
    [SerializeField, Min(0)] private float _exploadPause;
    
    private ParticleSystem _particleSystem;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_exploadPause <= 0)
            return;

            _exploadPause -= Time.deltaTime;
        if (_exploadPause <= 0)
            _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
