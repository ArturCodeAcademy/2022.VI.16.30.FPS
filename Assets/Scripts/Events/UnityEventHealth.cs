using System;
using UnityEngine.Events;

[Serializable]
public class UnityEventHealth : UnityEvent<HealthEventArgs> { }

public class HealthEventArgs : EventArgs
{
    public float CurrentHealth;
    public float MaxHealth;
}
