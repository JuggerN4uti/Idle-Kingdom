using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public Party PartyScript;
    public Missions MissionsScript;
    public int[] CommonHeroesCollected;
    public TMPro.TextMeshProUGUI GoldValue;

    [Header("Resources")]
    public int Gold;
    public int Stars;

    [Header("Stats")]
    public int PartyCount;
    public int BonusHealth, DamagePercentIncrease;

    [Header("Windows")]
    public GameObject[] WindowObject;

    [Header("Kings Upgrades")]
    public int[] StarsCosts;
    public Button[] KingUpgradeButton;
    public TMPro.TextMeshProUGUI[] UpgradeCostText;

    public void StartAdventure()
    {
        PartyScript.Open();
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
        for (int i = 0; i < KingUpgradeButton.Length; i++)
        {
            if (Stars >= StarsCosts[i])
                KingUpgradeButton[i].interactable = true;
            else KingUpgradeButton[i].interactable = false;
        }
    }

    public void BuyKingUpgrade(int which)
    {
        SpendStars(StarsCosts[which]);
        switch (which)
        {
            case 0:
                StarsCosts[0] *= 4;
                PartyCount++;
                break;
            case 1:
                StarsCosts[1]++;
                BonusHealth += 10;
                break;
            case 2:
                StarsCosts[2]++;
                DamagePercentIncrease++;
                break;
        }
        UpgradeCostText[which].text = StarsCosts[which].ToString("0");
        UpdateWindow(which);
    }
}
