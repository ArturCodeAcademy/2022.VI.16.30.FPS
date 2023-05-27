using System.Linq;
using UnityEngine;

public class AttackGrenade : Grenade
{
	[SerializeField, Min(0)] private float _exploadPause;
	[SerializeField] private GameObject _effect;
	[SerializeField] private AudioClip _audioExplosion;
	[SerializeField] private float _effectDestroyPause = 3;

	[Header("Damage")]
	[SerializeField] private AnimationCurve _damageDistanceMultiplier;
	[SerializeField, Min(0)] private float _damageDistance;
	[SerializeField, Min(0)] private float _damage;

	private void Update()
	{
		if (_exploadPause <= 0)
			return;

		_exploadPause -= Time.deltaTime;
		if (_exploadPause <= 0)
		{
			AudioSource.PlayClipAtPoint(_audioExplosion, transform.position);
			ApplyDamage();
			Destroy(Instantiate(_effect, transform.position, Quaternion.identity), _effectDestroyPause);
			Destroy(gameObject);
		}
	}

	private void ApplyDamage()
	{
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, _damageDistance, Vector3.up);

		foreach ((IHitable health, Transform t) in hits.Select(x => (x.transform.GetComponent<IHitable>() ?? x.transform.GetComponentInChildren<IHitable>(), x.transform))
			.Where(x => x.Item1 != null))
		{
			float distance = Vector3.Distance(t.position, transform.position);
			float multiplier = _damageDistanceMultiplier.Evaluate(distance / _damageDistance);
			float damage = _damage * multiplier;
			health?.Hit(damage);
		}
	}
}
