using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public Sprite UnitSprite, UnitPortrait;

    [Header("Stats")]
    public int HP, AR, AD;
    public float AS; //dps;

    [Header("Gain")]
    public float hpGain;
    public float arGain, adGain, asGain;

    float temp;

    public int TotalHP(int level)
    {
        temp = hpGain * (level - 1);
        return HP + Mathf.FloorToInt(temp);
    }

    public int TotalAR(int level)
    {
        temp = arGain * (level - 1);
        return AR + Mathf.FloorToInt(temp);
    }

    public float TotalAD(int level)
    {
        return AD + adGain * (level - 1);
    }

    public float TotalAS(int level)
    {
        return AS * (1f + asGain * (level - 1));
    }
}
