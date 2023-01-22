using UnityEngine;

public class GunBase : MonoBehaviour, IGun
{
    [field: SerializeField] public Vector3 NormalPosition { get; private set; }
    [field: SerializeField] public Quaternion NormalRotation { get; private set; }

    [field: SerializeField] public Vector3 AimingPosition { get; private set; }
    [field: SerializeField] public Quaternion AimingRotation { get; private set; }

    [field: SerializeField, Min(0.05f)] public float AimingTransitionDuration { get; private set; }
    [field: SerializeField] public AnimationCurve AimingTransitionCurve { get; private set; }
}
