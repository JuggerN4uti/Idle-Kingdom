using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public Party PartyScript;
    public Missions MissionsScript;
    public bool[] CommonHeroUnlocked;
    public int[] CommonHeroesCollected;
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
            CommonHeroUnlocked[HeroID] = true;
        else CommonHeroesCollected[HeroID]++;
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
            WindowObject[which].SetActive(true);
            UpdateWindow(which);
        }
    }

    void UpdateWindow(int which)
    {
        if (Stars >= StarsCosts[0] - StarsSpent / 5)
            KingUpgradeButton[0].interactable = true;
        else KingUpgradeButton[0].interactable = false;

        for (int i = 1; i < KingUpgradeButton.Length; i++)
        {
            if (Stars >= StarsCosts[i])
                KingUpgradeButton[i].interactable = true;
            else KingUpgradeButton[i].interactable = false;
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
        UpdateWindow(which);
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
}
