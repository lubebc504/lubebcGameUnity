using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect_BleedShoot : CardEffect
{
    public override void OnHit(Bullet bullet, GameObject target)
    {
        if (target.TryGetComponent<EnemyUnit>(out var enemy))
        {
            float bleedBonus = 0f;
            foreach (var relic in PlayerController.instance.relics)
            {
                bleedBonus += relic.GetBleedBonus();
            }

            float bleedDmg = 1f + bleedBonus;

            float bleedDuration = 3f;
            float bleedInterval = 0.5f;
            enemy.ApplyBleed(bleedDmg, bleedDuration, bleedInterval);
        }
    }
}