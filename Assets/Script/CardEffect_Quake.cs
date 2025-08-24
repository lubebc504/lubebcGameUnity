using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect_Quake : CardEffect
{
    public GameObject quakeFieldPrefab;

    private void Awake()
    {
        quakeFieldPrefab = Resources.Load<GameObject>("Prefabs/QuakeField");
    }

    public override void OnHit(Bullet bullet, GameObject target)
    {
        Vector3 hitPos = bullet.transform.position;

        GameObject fieldobj = Instantiate(quakeFieldPrefab, hitPos, Quaternion.identity);

        QuakeField field = fieldobj.GetComponent<QuakeField>();

        if (field != null)
        {
            field.Init(duration: 5f, damagePerSecond: 2f, tickInterval: 1f, radius: 3f);
        }
    }
}