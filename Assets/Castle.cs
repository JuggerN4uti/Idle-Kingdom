using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public Party PartyScript;
    public Missions MissionsScript;
    public TMPro.TextMeshProUGUI GoldValue, LumberValue;

    [Header("Resources")]
    public int Gold;
    public int Stars, Lumber;
    public float LumberPerSecond;

    [Header("Chests")]
    public int ChestsStored;
    public Button ChestButton;
    public TMPro.TextMeshProUGUI ChestsCount;

    [Header("Stats")]
    public int PartyCount;
    public int BonusHealth, DamagePercentIncrease;

    [Header("Windows")]
    public GameObject[] WindowObject;

    [Header("Army")]
    public bool[] CommonHeroUnlocked;
    public int[] CommonHeroesCollected, CommonHeroLevel;
    public Image[] CommonHeroImage, CommonHeroProgressFill;
    public TMPro.TextMeshProUGUI[] CommonHeroLevelText, CommonHeroProgressText;
    public Button[] CommonHeroLevelUpButton;

    [Header("Kings Upgrades")]
    public int[] StarsCosts;
    public int StarsSpent;
    public Button[] KingUpgradeButton;
    public TMPro.TextMeshProUGUI[] UpgradeCostText;

    public void StartAdventure()
    {
        PartyScript.Open();
    }

    public void CollectCommonHero(int HeroID)
    {
        if (!CommonHeroUnlocked[HeroID])
        {
            CommonHeroUnlocked[HeroID] = true;
            CommonHeroImage[HeroID].color = new Color(1f, 1f, 1f, 1f);
            CommonHeroLevel[HeroID] = 1;
            CommonHeroLevelText[HeroID].text = CommonHeroLevel[HeroID].ToString("0");
        }
        else CommonHeroesCollected[HeroID]++;

        CheckHero(HeroID);
    }

    public void GainGold(int amount)
    {
        Gold += amount;
        GoldValue.text = Gold.ToString("0");
        if (MissionsScript.MissionActive[1])
            MissionsScript.ProgressMissionID(1, amount);
    }

    public void SpendGold(int amount)
    {
        Gold -= amount;
        GoldValue.text = Gold.ToString("0");
    }

    public void GainStars(int amount)
    {
        Stars += amount;
        //GoldValue.text = Gold.ToString("0");
    }

    public void SpendStars(int amount)
    {
        Stars -= amount;
        //GoldValue.text = Gold.ToString("0");
    }

    public void GainLumber(int amount)
    {
        Lumber += amount;
        LumberValue.text = Lumber.ToString("0");
        if (MissionsScript.MissionActive[3])
            MissionsScript.ProgressMissionID(3, amount);
    }

    public void GetAChest()
    {
        ChestsStored++;
        ChestButton.interactable = true;
        ChestsCount.text = ChestsStored.ToString("0");
    }

    public void OpenChest()
    {
        CollectCommonHero(Random.Range(0, 4));
        ChestsStored--;
        if (ChestsStored == 0)
            ChestButton.interactable = false;
        ChestsCount.text = ChestsStored.ToString("0");
    }

    public void SpendLumber(int amount)
    {
        Lumber -= amount;
        LumberValue.text = Lumber.ToString("0");
    }

    public void OpenWindow(int which)
    {
        if (WindowObject[which].activeSelf)
            WindowObject[which].SetActive(false);
        else // potem jak bêdzie wiêcej okien
        {
            for (int i = 0; i < WindowObject.Length; i++)
            {
                WindowObject[i].SetActive(false);
            }
            WindowObject[which].SetActive(true);
            UpdateWindow(which);
        }
    }

    void UpdateWindow(int which)
    {
        switch (which)
        {
            case 0:
                if (Stars >= StarsCosts[0] - StarsSpent / 5)
                    KingUpgradeButton[0].interactable = true;
                else KingUpgradeButton[0].interactable = false;

                for (int i = 1; i < KingUpgradeButton.Length; i++)
                {
                    if (Stars >= StarsCosts[i])
                        KingUpgradeButton[i].interactable = true;
                    else KingUpgradeButton[i].interactable = false;
                }
                break;
            case 1:
                break;
        }
    }

    public void BuyKingUpgrade(int which)
    {
        if (which == 0)
        {
            SpendStars(StarsCosts[which] - StarsSpent / 5);
            StarsSpent = StarsSpent % 5;
        }
        else
        {
            SpendStars(StarsCosts[which]);
            StarsSpent += StarsCosts[which];
        }

        switch (which)
        {
            case 0:
                StarsCosts[0] *= 4;
                PartyCount++;
                break;
            case 1:
                StarsCosts[1]++;
                BonusHealth += 12;
                break;
            case 2:
                StarsCosts[2]++;
                DamagePercentIncrease += 2;
                break;
        }
        UpgradeCostText[0].text = (StarsCosts[0] - StarsSpent / 5).ToString("0");
        UpgradeCostText[which].text = StarsCosts[which].ToString("0");
        UpdateWindow(0);
    }

    public void SetLumberjackCamp(float lumberGain)
    {
        LumberPerSecond = lumberGain;
        Invoke("ChopTree", 1f / LumberPerSecond);
    }

    void ChopTree()
    {
        GainLumber(1);
        Invoke("ChopTree", 1f / LumberPerSecond);
    }

    void CheckHero(int which)
    {
        CommonHeroProgressText[which].text = CommonHeroesCollected[which].ToString("0") + "/" + (CommonHeroLevel[which] + 1).ToString("0");
        CommonHeroProgressFill[which].fillAmount = (CommonHeroesCollected[which] * 1f) / ((CommonHeroLevel[which] + 1) * 1f);
        if (CommonHeroesCollected[which] >= CommonHeroLevel[which] + 1)
        {
            CommonHeroProgressFill[which].color = new Color(0.1f, 1f, 0.1f, 1f);
            CommonHeroLevelUpButton[which].interactable = true;
        }
        else
        {
            CommonHeroProgressFill[which].color = new Color(1f, 1f, 1f, 1f);
            CommonHeroLevelUpButton[which].interactable = false;
        }
    }

    public void LevelUpHero(int which)
    {
        CommonHeroLevel[which]++;
        CommonHeroesCollected[which] -= CommonHeroLevel[which];
        CommonHeroLevelText[which].text = CommonHeroLevel[which].ToString("0");
        CheckHero(which);
        GainStars(1 + CommonHeroLevel[which] / 3);
    }
}
