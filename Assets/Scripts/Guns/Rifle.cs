using UnityEngine;

public class Rifle : GunBase
{
	protected override void Update()
	{
		base.Update();

		if (Input.GetMouseButton(0))
		{
			Shoot(_damage, _normalSpread, _penetratingPower, _impulsePower);
		}
	}
}
