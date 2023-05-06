using System;
using UnityEngine;

public interface IGun
{
    public event Action OnShoot;
    public event Action<float> OnStartReloading;
    public event Action OnDropReloading;
    public event Action OnReloaded;

    public Vector3 NormalPosition { get; }
    public Quaternion NormalRotation { get; }

    public Vector3 AimingPosition { get; }
    public Quaternion AimingRotation { get; }

    public float AimingTransitionDuration { get; }
    public AnimationCurve AimingTransitionCurve { get; }

    [field: SerializeField] public int AmmoCountInGun { get; }
    [field: SerializeField] public AmmoTypes.Type AmmoType { get; }
}
