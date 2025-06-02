using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicEffect_BleedBoost : RelicEffect
{
    public override float GetBleedBonus()
    {
        return 1f * stack;
    }
}