using UnityEngine;

public class Pistol : GunBase
{
    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot(_damage, _normalSpread, _penetratingPower, _impulsePower);
        }
    }
}
