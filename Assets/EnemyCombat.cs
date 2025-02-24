using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour
{
    public Adventure AdventureScript;

    public int ID, AD;
    public int[] gold;
    public float HitPoints, MaxHealth, AS, energy;
    bool active;

    public Image HealthBarFill, EnergyBarFill, EnemySprite;

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

    public void SetEnemy(Encounter EncounterMob, int whichMob)
    {
        EnemySprite.sprite = EncounterMob.MobsSprites[whichMob];
        MaxHealth = EncounterMob.MobsHP[whichMob] * 1f;
        HitPoints = MaxHealth;
        HealthBarFill.fillAmount = 1f;
        AD = EncounterMob.MobsAD[whichMob];
        AS = EncounterMob.MobsAS[whichMob];
        gold[0] = EncounterMob.minGold[whichMob];
        gold[1] = EncounterMob.maxGold[whichMob];
        energy = 0f;
        EnergyBarFill.fillAmount = 0f;
        active = false;
    }

    public void Begin()
    {
        active = true;
    }

    void Attack()
    {
        energy -= 1f;
        AdventureScript.TakeDamage(AD * 1f);
    }

    public void TakeDamage(float amount)
    {
        //amount /= 1f + partyArmor * 0.01f;
        amount *= AdventureScript.damageMultiplyer;
        //Debug.Log(amount);
        HitPoints -= amount;
        HealthBarFill.fillAmount = HitPoints / MaxHealth;

        if (HitPoints <= 0f)
            Defeated();
    }

    void Defeated()
    {
        AdventureScript.GainGold(Random.Range(gold[0], gold[1] + 1));
        AdventureScript.EnemyAlive[ID] = false;
        AdventureScript.MobObject[ID].SetActive(false);
        AdventureScript.mobsCount--;
        if (AdventureScript.MissionsScript.MissionActive[0])
            AdventureScript.MissionsScript.ProgressMissionID(0, 1);
        if (AdventureScript.mobsCount <= 0)
            AdventureScript.EncounterCleared();
    }
}
