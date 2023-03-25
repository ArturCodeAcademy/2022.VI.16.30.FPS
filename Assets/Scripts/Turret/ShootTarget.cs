using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(AudioSource))]
public class ShootTarget : MonoBehaviour
{
	[Header("Params")]
	[SerializeField, Min(0)] private float _damage;
	[SerializeField, Min(0)] private float _shootDistance;
	[SerializeField, Min(0)] private float _cooldownDuration;

	[Header("Audio")]
	[SerializeField] private AudioClip _shootClip;

	[Header("Objects / Prefabs")]
	[SerializeField] private GameObject _shootEffect;
	[SerializeField] private Transform[] _guns;

	private LineRenderer _laser;
	private AudioSource _audioSource;

	private Coroutine _coroutine;

	void Start()
	{
		_laser = GetComponent<LineRenderer>();
		_audioSource = GetComponent<AudioSource>();
	}

	public void StartShooting(Transform target)
	{
		StopShooting();
		_coroutine = StartCoroutine(Shooting(target));
	}

	public void StopShooting()
	{
		if (_coroutine != null)
			StopCoroutine(_coroutine);
	}

	IEnumerator Shooting(Transform target)
	{
		WaitForSeconds waitCooldown = new WaitForSeconds(_cooldownDuration);
		WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
		IHitable health = target.GetComponent<IHitable>() ?? target.GetComponentInParent<IHitable>();

		while (true)
		{
			bool flag = true;
			do
			{
				Physics.Raycast(_laser.transform.position, _laser.transform.forward, out RaycastHit hit, _shootDistance);

				for (Transform t = hit.transform; t != null; t = t.parent)
				{
					if (t == target.transform)
					{
						flag = false;
					}
				}

				yield return waitFrame;
			}
			while (flag);

			health.Hit(_damage);
			_audioSource.PlayOneShot(_shootClip);
			foreach (var gun in _guns)
				Instantiate(_shootEffect, gun.position, gun.rotation);

			yield return waitCooldown;
		}
	}
}
