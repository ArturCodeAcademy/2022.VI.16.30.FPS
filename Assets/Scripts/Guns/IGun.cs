using UnityEngine;

public interface IGun
{
    public Vector3 NormalPosition { get; }
    public Quaternion NormalRotation { get; }

    public Vector3 AimingPosition { get; }
    public Quaternion AimingRotation { get; }

    public float AimingTransitionDuration { get; }
    public AnimationCurve AimingTransitionCurve { get; }
}
