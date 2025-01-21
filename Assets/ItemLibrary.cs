using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibrary : MonoBehaviour
{
    public Item[] Items;
    int roll;

    public int ProjectileItem()
    {
        do
        {
            roll = Random.Range(0, Items.Length);
        } while (!Items[roll].projectileScaling);
        return roll;
    }

    public int DamageItem()
    {
        do
        {
            roll = Random.Range(0, Items.Length);
        } while (!Items[roll].damageScaling);
        return roll;
    }

    public int FireRateItem()
    {
        do
        {
            roll = Random.Range(0, Items.Length);
        } while (!Items[roll].rateScaling);
        return roll;
    }

    public int AreaSizeItem()
    {
        do
        {
            roll = Random.Range(0, Items.Length);
        } while (!Items[roll].sizeScaling);
        return roll;
    }

    public int DurationItem()
    {
        do
        {
            roll = Random.Range(0, Items.Length);
        } while (!Items[roll].durationScaling);
        return roll;
    }
}
