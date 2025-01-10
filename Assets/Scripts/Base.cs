using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public float maxHitPoints;
    public float hitPoints;

    public Image HealthBarFill;

    public void TakeDamage(float value)
    {
        hitPoints -= value;
        HealthBarFill.fillAmount = hitPoints / maxHitPoints;
    }
}
