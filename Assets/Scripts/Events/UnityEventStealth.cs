using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityEventStealth : UnityEvent<StealthEventArgs> { }

public class StealthEventArgs : ScannerEventArgs
{
    public float ReactionTime;
    public float ForgetTargetTime;
    public float ElapsedReactionTime;
}
