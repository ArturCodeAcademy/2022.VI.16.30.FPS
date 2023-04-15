using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTypes : MonoBehaviour
{
	public static AmmoTypes Instance { get; private set; }

	[SerializeField] private List<Pair> _pairs;

	private Dictionary<Type, Sprite> _ammoSprites;

	private void Awake()
	{
		_ammoSprites = new Dictionary<Type, Sprite>();
		Instance = this;
		foreach (var pair in _pairs)
			_ammoSprites[pair.Type] = pair.Sprite;
	}

	public Sprite this[Type type]
	{
		get
		{
			_ammoSprites.TryGetValue(type, out var sprite);
			return sprite;
		}
	}

	public enum Type
	{
		SaW_40,
		ACP_45,
		LR_22,
		Shotshells,
		FN_5_7,
		BMG_50
	}

	[Serializable]
	public struct Pair
	{
		public Type Type;
		public Sprite Sprite;
	}
}
