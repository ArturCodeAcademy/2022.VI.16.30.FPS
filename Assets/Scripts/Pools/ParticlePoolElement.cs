using System;
using UnityEngine;

public class ParticlePoolElement : MonoBehaviour, IPoolElement
{
    public Action OnElementUsed { get; set; }
}
