using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCombat : MonoBehaviour
{
    public Adventure AdventureScript;

    [Header("Stats")]
    public int Level;
    public float AD, AS, energy, critChance;
    int attacks;
    float temp;
    bool active, crited;

    public Image EnergyBarFill, HeroSprite;

    [Header("Perks")]
    public bool hasPerk;
    public int perk;
    public float perkStrength;
    int frenzyStacks;
    float bleed;

    void Update()
    {
        if (active)
        {
            energy += AS * Time.deltaTime;
            if (energy >= 1f)
            {
                Attack();
            }
            EnergyBarFill.fillAmount = energy;
        }
    }

    public void SetHero(Hero aHero, int level)
    {
        HeroSprite.sprite = aHero.UnitSprite;
        Level = level;
        AD = aHero.TotalAD(Level);
        AS = aHero.TotalAS(Level);
        energy = 0f;
        critChance = 0f;
        EnergyBarFill.fillAmount = 0f;
        active = false;

        attacks = 0;
        bleed = 0f;
        if (aHero.hasPerk)
        {
            hasPerk = true;
            perk = aHero.perk;
            perkStrength = aHero.perkStrength;
            StartingPerk();
        }
        else hasPerk = false;
    }

    public void Begin()
    {
        active = true;
    }

    public void Stop()
    {
        active = false;
    }

    void Attack()
    {
        energy -= 1f;
        attacks++;
        if (AdventureScript.mobsCount > 0)
        {
            AdventureScript.Enemies[AdventureScript.PossibleTarget()].TakeDamage(AttackDamage(), crited, bleed);
            /*if (hasPerk && perk == 2)
                AdventureScript.Enemies[AdventureScript.roll].GainBleed(0.032f);*/
        }
        if (AdventureScript.MissionsScript.MissionActive[2])
            AdventureScript.MissionsScript.ProgressMissionID(2, 1);
        if (hasPerk && perk == 3 && frenzyStacks < 15)
        {
            AS += perkStrength;
            frenzyStacks++;
        }
    }

    float AttackDamage()
    {
        temp = AD;
        if (hasPerk && perk == 4)
            temp += AdventureScript.partyArmor * perkStrength;
        if (hasPerk && perk == 5 && attacks % 3 == 0)
            temp += AdventureScript.PartyAD() * perkStrength;
        if (hasPerk && perk == 6)
            temp += AdventureScript.partyMaxHealth * perkStrength;
        if (critChance >= Random.Range(0f, 1f))
        {
            temp *= 1.75f;
            crited = true;
        }
        else crited = false;
        temp *= Random.Range(0.95f, 1.05f);
        return temp;
    }

    void StartingPerk()
    {
        frenzyStacks = 0;
        switch (perk)
        {
            case 0:
                AdventureScript.GainHP(AdventureScript.partyArmor * perkStrength);
                break;
            case 1:
                critChance += perkStrength;
                break;
            case 2:
                bleed = perkStrength;
                break;
            case 4:
                AS *= 1.1f;
                break;
            case 7:
                AdventureScript.Standard(perkStrength);
                break;
        }
    }
}
