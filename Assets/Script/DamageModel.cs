using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �� Ŭ����
public class DamageModel
{
    public float baseDamage;

    public DamageModel()
    {
        baseDamage = 10f;
    }

    public DamageModel(float baseDamage)
    {
        this.baseDamage = baseDamage;
    }
}