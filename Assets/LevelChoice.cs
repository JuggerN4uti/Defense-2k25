using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChoice : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Rolls")]
    public int rarity;
    public int rareCharge;
    public int[] prize1roll, prize2roll;
    public float experienceBuff;

    [Header("HUD")]
    public GameObject PrizeHUD;
    public TMPro.TextMeshProUGUI[] FirstPrizeInfo, SecondPrizeInfo;
    public string[] CommonFirstTooltips, CommonSecondTooltips;

    public void SetChoices()
    {
        Time.timeScale = 0f;
        rareCharge++;
        if (rareCharge >= 5)
        {
            rareCharge -= 5;
            //SetRarePrizes();
            SetCommonPrizes();
        }
        else SetCommonPrizes();
    }

    void SetCommonPrizes()
    {
        rarity = 0;
        experienceBuff = 10f + 0.04f * PlayerScript.totalExperience;
        for (int i = 0; i < 3; i++)
        {
            prize1roll[i] = Random.Range(0, CommonFirstTooltips.Length);
            prize2roll[i] = Random.Range(0, CommonSecondTooltips.Length);
        }
        DisplayChoices();
    }

    void DisplayChoices()
    {
        PrizeHUD.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            FirstPrizeInfo[i].text = CommonFirstTooltips[prize1roll[i]];
            if (prize2roll[i] == 4)
                SecondPrizeInfo[i].text = "+" + experienceBuff.ToString("0") + " Experience";
            else SecondPrizeInfo[i].text = CommonSecondTooltips[prize2roll[i]];
        }
    }

    public void CollectPrize(int which)
    {
        GetFirstPrize(prize1roll[which]);
        GetSecondPrize(prize2roll[which]);
        Time.timeScale = 1f;
        PrizeHUD.SetActive(false);
    }

    void GetFirstPrize(int effect)
    {
        switch (rarity, effect)
        {
            case (0, 0):
                PlayerScript.fireRate /= 1.06f;
                break;
            case (0, 1):
                PlayerScript.damage += 0.25f;
                break;
            case (0, 2):
                PlayerScript.inaccuracy /= 1.08f;
                PlayerScript.force += 0.376f;
                break;
            case (0, 3):
                PlayerScript.magazineSize += 4;
                PlayerScript.bullets += 4;
                PlayerScript.MagazineInfo.text = PlayerScript.bullets.ToString() + "/" + PlayerScript.magazineSize.ToString();
                break;
            case (0, 4):
                rareCharge++;
                break;
        }
    }

    void GetSecondPrize(int effect)
    {
        switch (rarity, effect)
        {
            case (0, 0):
                PlayerScript.GainDamage(0.02f);
                break;
            case (0, 1):
                PlayerScript.GainRate(0.025f);
                break;
            case (0, 2):
                PlayerScript.GainSize(0.03f);
                break;
            case (0, 3):
                PlayerScript.GainDuration(0.03f);
                break;
            case (0, 4):
                PlayerScript.GainXP(experienceBuff);
                break;
            case (0, 5):
                PlayerScript.BaseScript.GainHP(10);
                PlayerScript.BaseScript.GainRegen(1);
                break;
        }
    }
}
