using UnityEngine;

public class ParticleEffectsPool : PoolBase<ParticlePoolElement>
{
    protected override ParticlePoolElement CreateNewElement()
    {
        return Instantiate(_prefabs[Random.Range(0, _prefabs.Length)].GetComponent<ParticlePoolElement>());
    }
}
