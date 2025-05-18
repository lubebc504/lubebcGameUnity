using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect_DoubleShoot : CardEffect
{
    public override void OnFire(Bullet bullet)
    {
        bullet.damageModel.baseDamage *= 2f;
        Debug.Log("DoubleShoot 발동! 데미지 2배로 증가됨.");
    }
}