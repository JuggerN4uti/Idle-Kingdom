using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Adventure : MonoBehaviour
{
    public Party PartyScript;
    public StagesLibrary SLib;
    public HeroCombat[] Heroes;
    public EnemyCombat[] Enemies;

    public GameObject AdventureHud;
    public GameObject[] HeroObject, MobObject;

    public int stage, encounter, heroesCount, mobsCount;
    public float partyHitPoints, partyMaxHealth, partyArmor;
    public bool[] EnemyAlive;
    public Image HealthBarFill;
    public TMPro.TextMeshProUGUI HealthValue;

    int roll;
    float temp;

    public void SetAdventure(int Stage)
    {
        AdventureHud.SetActive(true);

        stage = Stage; encounter = 0;
        heroesCount = PartyScript.PartyCount;
        mobsCount = SLib.Stages[stage].Encounters[encounter].MobsCount;
        partyMaxHealth = PartyScript.partyHP * 1f;
        partyHitPoints = partyMaxHealth;
        HealthValue.text = partyHitPoints.ToString("0") + "/" + partyMaxHealth.ToString("0");
        HealthBarFill.fillAmount = 1f;
        partyArmor = PartyScript.partyAR;

        for (int i = 0; i < 6; i++)
        {
            HeroObject[i].SetActive(false);
        }
        for (int i = 0; i < heroesCount; i++)
        {
            HeroObject[i].SetActive(true);
            Heroes[i].SetHero(PartyScript.PartyHeroes[i]);
        }

        SetEnemies();

        Invoke("Begin", 1.25f);
    }

    void SetEnemies()
    {
        for (int i = 0; i < 4; i++)
        {
            MobObject[i].SetActive(false);
            EnemyAlive[i] = false;
        }

        for (int i = 0; i < mobsCount; i++)
        {
            MobObject[i].SetActive(true);
            EnemyAlive[i] = true;
            Enemies[i].SetEnemy(SLib.Stages[stage].Encounters[encounter], i);
        }
    }

    void Begin()
    {
        for (int i = 0; i < heroesCount; i++)
        {
            Heroes[i].Begin();
        }
        for (int i = 0; i < mobsCount; i++)
        {
            Enemies[i].Begin();
        }
    }

    void Stop()
    {
        for (int i = 0; i < heroesCount; i++)
        {
            Heroes[i].Stop();
        }
    }

    public void TakeDamage(float amount)
    {
        amount /= 1f + partyArmor * 0.01f;
        partyHitPoints -= amount;
        HealthValue.text = partyHitPoints.ToString("0") + "/" + partyMaxHealth.ToString("0");
        HealthBarFill.fillAmount = partyHitPoints / partyMaxHealth;
    }

    public int PossibleTarget()
    {
        do
        {
            roll = Random.Range(0, 4);
        } while (!EnemyAlive[roll]);
        return roll;
    }

    public void EncounterCleared()
    {
        Stop();

        encounter++;
        mobsCount = SLib.Stages[stage].Encounters[encounter].MobsCount;
        SetEnemies();

        Invoke("Begin", 1f);
    }
}
