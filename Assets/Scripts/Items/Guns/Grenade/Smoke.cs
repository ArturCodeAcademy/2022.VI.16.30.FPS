using UnityEngine;

public class Smoke : Grenade
{
    [SerializeField, Min(0)] private float _exploadPause;
    
    private ParticleSystem _particleSystem;
    private AudioSource _audioSourceExplosion;
    private AudioSource _audioSourceSmoke;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
		_audioSourceExplosion = GetComponent<AudioSource>();
		_audioSourceSmoke = transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_exploadPause <= 0)
            return;

            _exploadPause -= Time.deltaTime;
        if (_exploadPause <= 0)
        {
            _particleSystem.Play();
			_audioSourceExplosion.Play();
			_audioSourceSmoke.Play();
		}
    }

    private void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
