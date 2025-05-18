using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데미지의 유형을 정의하는 열거형
public enum DamageType
{
    Normal,
    Fire,
    Freeze
}

// 데이터 모델 클래스
public class DamageModel
{
    public float baseDamage;
    public DamageType damageType;

    public DamageModel()
    {
        baseDamage = 10f;
        damageType = DamageType.Normal;
    }

    public DamageModel(float baseDamage, DamageType damageType)
    {
        this.baseDamage = baseDamage;
        this.damageType = damageType;
    }
}