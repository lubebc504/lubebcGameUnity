using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데이터 모델 클래스
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