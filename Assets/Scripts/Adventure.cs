using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Adventure : MonoBehaviour
{
    public Castle CastleScript;
    public Party PartyScript;
    public StagesLibrary SLib;
    public Missions MissionsScript;
    public HeroCombat[] Heroes;
    public EnemyCombat[] Enemies;

    public GameObject AdventureHud, ResultsHud;
    public GameObject[] HeroObject, MobObject;

    public int stage, encounter, heroesCount, mobsCount, goldCollected;
    public float partyHitPoints, partyMaxHealth, partyArmor, damageMultiplyer;
    public bool[] EnemyAlive;
    public Image HealthBarFill;
    public TMPro.TextMeshProUGUI HealthValue, GoldValue, ResultTitle, GoldSecured;

    int roll;
    float temp;

    public void SetAdventure(int Stage)
    {
        AdventureHud.SetActive(true);

        stage = Stage; encounter = 0; goldCollected = 0;
        damageMultiplyer = 1f + CastleScript.DamagePercentIncrease * 0.01f;
        GoldValue.text = goldCollected.ToString("0");
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
            Heroes[i].SetHero(PartyScript.PartyHeroes[i], PartyScript.HeroLevel[i]);
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

        if (partyHitPoints <= 0f)
            PartyDefeated();
    }

    void PartyDefeated()
    {
        Stop();

        Results(false);
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
        if (encounter >= SLib.Stages[stage].Encounters.Length)
            Results(true);
        else
        {
            mobsCount = SLib.Stages[stage].Encounters[encounter].MobsCount;
            SetEnemies();

            Invoke("Begin", 1f);
        }
    }

    public void GainGold(int amount)
    {
        goldCollected += amount;
        GoldValue.text = "+" + goldCollected.ToString("0");
    }

    void Results(bool cleared)
    {
        AdventureHud.SetActive(false);

        if (cleared)
            ResultTitle.text = "Your Party\nhas Prevailed!";
        else ResultTitle.text = "Your Party\nhas Failed..";
        GoldSecured.text = goldCollected.ToString("0");

        ResultsHud.SetActive(true);
    }

    public void ReturnToCamp()
    {
        CastleScript.GainGold(goldCollected);

        ResultsHud.SetActive(false);
    }
}
