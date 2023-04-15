using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBackpack : MonoBehaviour
{
	public event Action OnAmmoCountChanged;

	[SerializeField] private List<Pair> _beginAmmoCount;

	private Dictionary<AmmoTypes.Type, int> _ammo;

	private void Awake()
	{
		_ammo = new Dictionary<AmmoTypes.Type, int>();

		foreach (var pair in _beginAmmoCount)
			if (_ammo.ContainsKey(pair.Type))
				_ammo[pair.Type] += pair.Count;
			else
				_ammo[pair.Type] = pair.Count;
	}

	public int GetAmmoCount(AmmoTypes.Type type)
	{
        return _ammo.ContainsKey(type) ? _ammo[type] : 0;
    }

    public int GetAmmo(AmmoTypes.Type type, int count)
	{
		if (!_ammo.TryGetValue(type, out int value))
			return 0;
		int min = Math.Min(value, count);
		_ammo[type] -= min;
        OnAmmoCountChanged?.Invoke();
        return min;
	}

	public void AddAmmo(AmmoTypes.Type type, int value)
	{
		if (_ammo.ContainsKey(type))
			_ammo[type] += value;
		else
			_ammo[type] = value;

		OnAmmoCountChanged?.Invoke();
	}

	[Serializable]
	public struct Pair
	{
		public AmmoTypes.Type Type;
		public int Count;
	}
}
