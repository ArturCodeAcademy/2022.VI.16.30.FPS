using System;
using UnityEngine;

public class ScannerTarget : IEquatable<ScannerTarget>
{
    public Transform Target { get; set; }
    public bool IsVisible { get; set; }
    public CharacterType Type { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is ScannerTarget scannerTarget)
            return Equals(scannerTarget);
        return false;
    }

    public bool Equals(ScannerTarget other)
    {
         return Target.Equals(other.Target);
    }

    public override int GetHashCode()
    {
        return Target.GetHashCode();
    }
}
