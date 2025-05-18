using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public static void DealDamage(IDamageable damageable, DamageModel damageModel)
    {
        damageable.TakeDamage(damageModel);
    }
}