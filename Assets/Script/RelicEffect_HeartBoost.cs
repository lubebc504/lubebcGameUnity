using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicEffect_HeartBoost : RelicEffect
{
    public override void OnEquip(PlayerController player)
    {
        player.maxHealth *= 1.2f;
        player.playerhp *= 1.2f;
    }
}