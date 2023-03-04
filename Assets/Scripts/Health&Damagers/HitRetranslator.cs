using UnityEngine;

public class HitRetranslator : MonoBehaviour, IHitable
{
	[SerializeField] private Health _hitTarget; 
	[SerializeField] private float _damageMultiplier; 

	public float Hit(float damage)
	{
		return _hitTarget.Hit(damage * _damageMultiplier);
	}

	public void Kill()
	{
		_hitTarget.Kill();
	}
}
