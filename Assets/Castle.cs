using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public Party PartyScript;
    public int Gold;
    public int[] CommonHeroesCollected;
    public TMPro.TextMeshProUGUI GoldValue;


    public void StartAdventure()
    {
        PartyScript.Open();
    }

    public void GainGold(int amount)
    {
        Gold += amount;
        GoldValue.text = Gold.ToString("0");
    }
}
