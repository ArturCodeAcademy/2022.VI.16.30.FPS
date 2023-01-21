using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityEventScanner : UnityEvent<ScannerEventArgs> {}

public class ScannerEventArgs : EventArgs
{
    public Transform Sender;
    public Transform Target;
    public CharacterType.Type TargetType;
}
