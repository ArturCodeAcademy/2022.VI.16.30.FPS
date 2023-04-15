using UnityEngine;

[RequireComponent(typeof(GunBase))]
public class GunSounds : MonoBehaviour
{
    [SerializeField] private AudioClip _shoot;
    [SerializeField] private AudioClip _reload;
    
    private AudioSource _source;
	private GunBase _gun;

	private void Awake()
	{
		_source = GetComponentInChildren<AudioSource>();
		_gun = GetComponent<GunBase>();
	}

	private void Start()
	{
		_gun.OnShoot += OnShoot;
		_gun.OnStartReloading += OnReload;
		_gun.OnReloaded += OnDropOrEndReloading;
		_gun.OnDropReloading += OnDropOrEndReloading;
	}

	private void OnShoot()
	{
		_source.PlayOneShot(_shoot);
	}

	private void OnReload(float _)
	{
		_source.PlayOneShot(_reload);
	}

	private void OnDropOrEndReloading()
	{
		_source.Pause();
	}

	private void OnDestroy()
	{
		_gun.OnShoot -= OnShoot;
		_gun.OnStartReloading -= OnReload;
		_gun.OnReloaded -= OnDropOrEndReloading;
		_gun.OnDropReloading -= OnDropOrEndReloading;
	}
}
