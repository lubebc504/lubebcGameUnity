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
                bleedBonus += relic.GetBleedBonus();//나중에 여기말고 enemy쪽에 옮기는게 나을지도?
            }

            float bleedDmg = 1f + bleedBonus;

            float bleedDuration = 3f;
            float bleedInterval = 0.5f;
            enemy.ApplyBleed(bleedDmg, bleedDuration, bleedInterval);
        }
    }
}