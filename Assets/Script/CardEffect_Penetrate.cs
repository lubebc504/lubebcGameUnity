using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect_Penetrate : CardEffect
{
    public override void OnFire(Bullet bullet)
    {
        bullet.canPenetrate = true;
    }
}