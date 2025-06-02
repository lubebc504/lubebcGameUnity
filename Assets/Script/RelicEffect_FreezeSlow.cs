using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicEffect_FreezeSlow : RelicEffect
{
    public override void OnHit(PlayerController player, EnemyUnit enemy)
    {
        if (!enemy.isSlowed)
        {
            enemy.isSlowed = true;
            float originalSpeed = enemy.speed;
            for (int i = 0; i < stack; i++)
            {
                enemy.speed *= 0.8f;
            }
            StartCoroutine(RollBackSpeed(enemy, originalSpeed));
        }
    }

    public IEnumerator RollBackSpeed(EnemyUnit enemy, float speed)
    {
        yield return new WaitForSeconds(2f);
        enemy.speed = speed;
        enemy.isSlowed = false;
    }
}