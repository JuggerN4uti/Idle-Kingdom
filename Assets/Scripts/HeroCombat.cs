using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCombat : MonoBehaviour
{
    public Adventure AdventureScript;

    public int Level;
    public float AD, AS, energy;
    bool active;

    public Image EnergyBarFill, HeroSprite;

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
        EnergyBarFill.fillAmount = 0f;
        active = false;
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
        if (AdventureScript.mobsCount > 0)
            AdventureScript.Enemies[AdventureScript.PossibleTarget()].TakeDamage(AD);
        if (AdventureScript.MissionsScript.MissionActive[2])
            AdventureScript.MissionsScript.ProgressMissionID(2, 1);
    }
}
