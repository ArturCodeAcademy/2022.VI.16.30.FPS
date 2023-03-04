using UnityEngine;

public class Shotgun : GunBase
{
	[SerializeField, Min(1)] protected int _bulletsCountPerShoot;

	protected override void Update()
	{
		base.Update();

		if (Input.GetMouseButtonDown(0))
		{
			Shoot(_damage, _normalSpread, _penetratingPower, _impulsePower, _bulletsCountPerShoot);
		}
	}

	protected override void FillMagazine()
	{
		int got = _ammoBackpack.GetAmmo(AmmoType, 1);
		AmmoCountInGun += got;
		InvokeOnReloaded();
	}
}
