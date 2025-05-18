using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect_BleedShoot : CardEffect
{
    public override void OnHit(Bullet bullet, GameObject target)
    {
        if (target.TryGetComponent<EnemyUnit>(out var enemy))
        {
            float bleedDmg = 1f;
            float bleedDuration = 3f;
            float bleedInterval = 0.5f;
            enemy.ApplyBleed(bleedDmg, bleedDuration, bleedInterval);
        }
    }
}