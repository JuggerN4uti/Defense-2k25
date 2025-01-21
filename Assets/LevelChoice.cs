using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChoice : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public Base BaseScript;
    public ItemLibrary ILib;

    [Header("Rolls")]
    public int rarity;
    public int uncommonCharge, rareCharge;
    public int[] prize1roll, prize2roll, rareItem;
    public float experienceBuff;

    [Header("HUD")]
    public GameObject PrizeHUD;
    public Image[] ChoiceRarityImage;
    public Sprite[] RaritySprites;
    public TMPro.TextMeshProUGUI[] FirstPrizeInfo, SecondPrizeInfo;
    public string[] CommonFirstTooltips, CommonSecondTooltips, UncommonFirstTooltips, UncommonSecondTooltips, RareFirstTooltips, RareSecondTooltips;

    public void SetChoices()
    {
        Time.timeScale = 0f;
        uncommonCharge += Random.Range(2, 6);
        rareCharge++;
        if (rareCharge >= 5)
        {
            rareCharge -= 5;
            SetRarePrizes();
        }
        else if (uncommonCharge >= 8)
        {
            uncommonCharge -= 8;
            SetUncommonPrizes();
        }
        else SetCommonPrizes();
    }

    void SetCommonPrizes()
    {
        rarity = 0;
        experienceBuff = 12f + 0.044f * PlayerScript.totalExperience;
        for (int i = 0; i < 3; i++)
        {
            ChoiceRarityImage[i].sprite = RaritySprites[0];
            prize1roll[i] = Random.Range(0, CommonFirstTooltips.Length);
            prize2roll[i] = Random.Range(0, CommonSecondTooltips.Length);
        }
        DisplayChoices();
    }

    void SetUncommonPrizes()
    {
        rarity = 1;
        for (int i = 0; i < 3; i++)
        {
            ChoiceRarityImage[i].sprite = RaritySprites[1];
            prize1roll[i] = Random.Range(0, UncommonFirstTooltips.Length);
            prize2roll[i] = Random.Range(0, UncommonSecondTooltips.Length);
        }
        DisplayChoices();
    }

    void SetRarePrizes()
    {
        rarity = 2;
        for (int i = 0; i < 3; i++)
        {
            ChoiceRarityImage[i].sprite = RaritySprites[2];
            prize1roll[i] = Random.Range(0, RareFirstTooltips.Length);
            prize2roll[i] = Random.Range(0, RareSecondTooltips.Length);
            switch (prize2roll[i])
            {
                case 0:
                    rareItem[i] = ILib.ProjectileItem();
                    break;
                case 1:
                    rareItem[i] = ILib.DamageItem();
                    break;
                case 2:
                    rareItem[i] = ILib.FireRateItem();
                    break;
                case 3:
                    rareItem[i] = ILib.AreaSizeItem();
                    break;
                case 4:
                    rareItem[i] = ILib.DurationItem();
                    break;
            }
        }
        DisplayChoices();
    }

    void DisplayChoices()
    {
        PrizeHUD.SetActive(true);

        switch (rarity)
        {
            case 0:
                for (int i = 0; i < 3; i++)
                {
                    FirstPrizeInfo[i].text = CommonFirstTooltips[prize1roll[i]];
                    if (prize2roll[i] == 4)
                        SecondPrizeInfo[i].text = "+" + experienceBuff.ToString("0") + " Experience";
                    else SecondPrizeInfo[i].text = CommonSecondTooltips[prize2roll[i]];
                }
                break;
            case 1:
                for (int i = 0; i < 3; i++)
                {
                    FirstPrizeInfo[i].text = UncommonFirstTooltips[prize1roll[i]];
                    SecondPrizeInfo[i].text = UncommonSecondTooltips[prize2roll[i]];
                }
                break;
            case 2:
                for (int i = 0; i < 3; i++)
                {
                    FirstPrizeInfo[i].text = RareFirstTooltips[prize1roll[i]];
                    SecondPrizeInfo[i].text = RareSecondTooltips[prize2roll[i]] + " & Get " + ILib.Items[rareItem[i]].ItemName;
                }
                break;
        }
    }

    public void CollectPrize(int which)
    {
        GetFirstPrize(prize1roll[which]);
        GetSecondPrize(prize2roll[which]);
        if (rarity == 2)
            BaseScript.CollectItem(rareItem[which]);
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
                PlayerScript.inaccuracy /= 1.09f;
                PlayerScript.force += 0.564f;
                break;
            case (0, 3):
                PlayerScript.GainMagazineSize(4);
                break;
            case (0, 4):
                uncommonCharge += Random.Range(2, 4);
                rareCharge++;
                break;
            case (1, 0):
                PlayerScript.fireRate /= 1.048f;
                PlayerScript.damage += 0.2f;
                break;
            case (1, 1):
                PlayerScript.damage += 0.2f;
                PlayerScript.inaccuracy /= 1.072f;
                PlayerScript.force += 0.451f;
                break;
            case (1, 2):
                PlayerScript.inaccuracy /= 1.072f;
                PlayerScript.force += 0.451f;
                PlayerScript.GainMagazineSize(3);
                break;
            case (1, 3):
                PlayerScript.GainMagazineSize(3);
                PlayerScript.fireRate /= 1.048f;
                break;
            case (2, 0):
                PlayerScript.fireRate *= 1.25f;
                PlayerScript.damageRatio += 0.4f;
                break;
            case (2, 1):
                PlayerScript.pierce++;
                break;
            case (2, 2):
                PlayerScript.damage += 0.05f * PlayerScript.magazineSize;
                PlayerScript.dmgPerMag++;
                PlayerScript.GainMagazineSize(2);
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
                BaseScript.GainHP(10);
                BaseScript.GainRegen(1);
                break;
            case (1, 0):
                PlayerScript.GainDamage(0.032f);
                break;
            case (1, 1):
                PlayerScript.GainRate(0.04f);
                break;
            case (1, 2):
                PlayerScript.GainSize(0.048f);
                break;
            case (1, 3):
                PlayerScript.GainDuration(0.048f);
                break;
            case (1, 4):
                BaseScript.GainHP(16);
                BaseScript.GainRegen(2);
                break;
            case (2, 0):
                PlayerScript.projectileCountIncrease++;
                break;
            case (2, 1):
                PlayerScript.GainDamage(0.06f);
                break;
            case (2, 2):
                PlayerScript.GainRate(0.075f);
                break;
            case (2, 3):
                PlayerScript.GainSize(0.09f);
                break;
            case (2, 4):
                PlayerScript.GainDuration(0.09f);
                break;
        }
    }
}
