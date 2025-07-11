using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{
    public CardData data;

    public virtual void OnFire(Bullet bullet)
    { }

    public virtual void OnHit(Bullet bullet, GameObject target)
    { }
}