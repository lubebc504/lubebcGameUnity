using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ������ �����ϴ� ������
public enum DamageType
{
    Normal,
    Fire,
    Freeze
}

// ������ �� Ŭ����
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