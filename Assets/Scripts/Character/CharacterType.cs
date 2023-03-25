using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterType : MonoBehaviour, IEquatable<CharacterType>
{
    [field: SerializeField] public Type ObjectType;
    public static event Action<Type> OnCharactersCountChanged;
    
    private static HashSet<CharacterType> s_charactersPool = new HashSet<CharacterType>();

    private void OnEnable()
    {
        if (!s_charactersPool.Contains(this))
        {
            s_charactersPool.Add(this);
            OnCharactersCountChanged?.Invoke(ObjectType);
        }
    }

    private void OnDisable()
    {
        if (s_charactersPool.Contains(this))
        {
            s_charactersPool.Remove(this);
            OnCharactersCountChanged?.Invoke(ObjectType);
        }
    }

    public static IEnumerable<CharacterType> GetAllObjectsOfType(Type type)
    {
        return s_charactersPool?.Where(x => type.HasFlag(x.ObjectType));
    }

    public bool Equals(CharacterType other)
    {
        return transform == other.transform;
    }

    public override bool Equals(object obj)
    {
        if (obj is CharacterType characterType)
            return Equals(characterType);
        return false;
    }

    public override int GetHashCode()
    {
        return transform.GetHashCode();
    }

    public enum Type
    {
        Player = 0b001,
        Turret = 0b010,
        Zombie = 0b100
    }
}
