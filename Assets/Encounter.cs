using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    public int MobsCount;

    public Sprite[] MobsSprites;

    public int[] MobsHP, MobsAD;
    public float[] MobsAS;

    public int[] minGold, maxGold;

    public bool checkDiff;
    float temp, temp2;

    void Start()
    {
        if (checkDiff)
            CalculateDifficulty();
    }

    void CalculateDifficulty()
    {
        for (int i = 0; i < MobsCount; i++)
        {
            temp += MobsHP[i];
        }
        Debug.Log("HP = " + temp);

        for (int i = 0; i < MobsCount; i++)
        {
            temp2 += MobsAD[i] * MobsAS[i];
        }
        Debug.Log("DPS = " + temp);
        temp *= temp2;
        temp /= 0.75f + 0.25f * MobsCount;
        Debug.Log("Diff = " + temp);
    }
}
