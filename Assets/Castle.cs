using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public Party PartyScript;
    public int[] CommonHeroesCollected;

    public void StartAdventure()
    {
        PartyScript.Open();
    }
}
