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
    public int fifthOfGold, Lumber, GoldPer10Second;
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
    public int[] UpgradeCosts;
    public int GoldSpentOnUpgrades;
    public Button[] KingUpgradeButton;
    public TMPro.TextMeshProUGUI[] UpgradeCostText;

    void Start()
    {
        Invoke("PassiveGold", 2f);
    }

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
            GoldPer10Second++;
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

    void PassiveGold()
    {
        fifthOfGold += GoldPer10Second;
        if (fifthOfGold >= 5)
        {
            GainGold(fifthOfGold / 5);
            fifthOfGold = fifthOfGold % 5;
        }
        Invoke("PassiveGold", 2f);
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
                if (Gold >= UpgradeCosts[0] - GoldSpentOnUpgrades / 5)
                    KingUpgradeButton[0].interactable = true;
                else KingUpgradeButton[0].interactable = false;

                for (int i = 1; i < KingUpgradeButton.Length; i++)
                {
                    if (Gold >= UpgradeCosts[i])
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
            SpendGold(UpgradeCosts[which] - GoldSpentOnUpgrades / 5);
            GoldSpentOnUpgrades = GoldSpentOnUpgrades % 5;
        }
        else
        {
            SpendGold(UpgradeCosts[which]);
            GoldSpentOnUpgrades += UpgradeCosts[which];
        }

        switch (which)
        {
            case 0:
                UpgradeCosts[0] *= 4;
                PartyCount++;
                break;
            case 1:
                UpgradeCosts[1] += 10;
                BonusHealth += 12;
                break;
            case 2:
                UpgradeCosts[2] += 10;
                DamagePercentIncrease += 2;
                break;
        }
        UpgradeCostText[0].text = (UpgradeCosts[0] - GoldSpentOnUpgrades / 5).ToString("0");
        UpgradeCostText[which].text = UpgradeCosts[which].ToString("0");
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
        GainGold(CommonHeroLevel[which]);
        GoldPer10Second++;
    }
}
