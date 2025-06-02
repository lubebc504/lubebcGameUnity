using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicEffect : MonoBehaviour
{
    public RelicData data;
    public int stack = 1;

    public virtual void OnEquip(PlayerController player)
    { }

    public virtual void OnStack(PlayerController player)
    {
        stack++;
        OnEquip(player); // ���� �ݿ�
    }

    public virtual void OnKill(PlayerController player, EnemyUnit enemy)
    { }

    public virtual void OnUpdate(PlayerController player)
    { }

    public virtual void OnBulletFire(Bullet bullet)
    { }

    public virtual float GetBleedBonus()
    {
        return 0f; // �⺻ ������ ������ ���� ����
    }

    public virtual void OnHit(PlayerController player, EnemyUnit enemy)
    {
    }
}