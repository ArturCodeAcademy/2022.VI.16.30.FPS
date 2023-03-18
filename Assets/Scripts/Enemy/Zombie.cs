using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(ZombiePatrol), typeof(ZombieAttack))]
public class Zombie : MonoBehaviour
{
	public bool IsReacted { get; private set; }

	[SerializeField, Min(0)] private float _alertDistance;
	[SerializeField, Min(0)] private StealthForPlayer _stealth;

	private static HashSet<Zombie> _zombiesPool;

	private ZombiePatrol _patrol;
	private ZombieAttack _attack;
	private Health _health;

	private void Awake()
	{
		IsReacted = false;
		_zombiesPool ??= new HashSet<Zombie>();
		_patrol = GetComponent<ZombiePatrol>();
		_attack = GetComponent<ZombieAttack>();
		_health = GetComponent<Health>();
	}

	private void Start()
	{
		_patrol.Begin();
	}

	private void OnEnable()
	{
		_stealth.OnReact.AddListener(OnRact);
		_stealth.OnCalmDown.AddListener(OnCalmDown);
		_health.OnHealthValueChanged.AddListener(OnHealthValueChanged);
		_zombiesPool.Add(this);
	}

	private void OnHealthValueChanged(HealthEventArgs arg)
	{
		OnRact();
	}

	private void OnCalmDown(StealthEventArgs arg)
	{
		IsReacted = false;
		_attack.Stop();
		_patrol.Stop();
		_patrol.Begin();
	}

	private void OnRact(StealthEventArgs arg = default)
	{
		IsReacted = true;
		_patrol.Stop();
		_attack.Stop();
		_attack.Begin();

		foreach (Zombie zombie in _zombiesPool)
			if (!zombie.IsReacted &&
				Vector3.Distance(transform.position, zombie.transform.position)
				< _alertDistance)
				zombie.OnRact();
	}

	private void OnDisable()
	{
		_stealth.OnReact.RemoveListener(OnRact);
		_stealth.OnCalmDown.RemoveListener(OnCalmDown);
		_health.OnHealthValueChanged.RemoveListener(OnHealthValueChanged);
		_zombiesPool.Remove(this);
	}
}
