using System;
using UnityEngine.Events;

[Serializable]
public class UnityEventOnGetGrounded : UnityEvent<GroundedEventArgs> { }

public class GroundedEventArgs : EventArgs
{
    public float VerticalVelocity;
}
