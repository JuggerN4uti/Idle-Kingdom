using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wizard : MonoBehaviour
{
    [Header("Scripts")]
    public Castle CastleScript;
    public HeroesLibrary HLib;

    [Header("Mana")]
    public float mana;
    public float maxMana, manaRegen, manaPerClick;

    [Header("Summons")]
    public int UncommonChance;
    public GameObject SummonPrefab;
    public Transform SummonSpot;
    Summoned SummonedScript;
    int roll;

    [Header("Level")]
    public int level;
    public int experience, experienceRequired;
    public TMPro.TextMeshProUGUI LevelValue;
    public Image ExperienceBarFill;

    [Header("UI")]
    public TMPro.TextMeshProUGUI ManaValue;
    public Image ManaBarFill;
    public Button SummonButton;

    void Start()
    {
        Invoke("ManaRegen", 1f);
        experienceRequired = NextLevelExp();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GainMana(manaPerClick);
    }

    void ManaRegen()
    {
        GainMana(manaRegen);
        Invoke("ManaRegen", 1f);
    }

    void GainMana(float manaGained, bool overflow = false)
    {
        if (overflow)
        {
            mana += manaGained;
        }
        else if (mana <= maxMana)
        {
            mana += manaGained;
            if (mana >= maxMana)
                mana = maxMana;
        }
        ManaValue.text = mana.ToString("0") + "/" + maxMana.ToString("0");
        ManaBarFill.fillAmount = mana / maxMana;
        if (mana >= 75f)
            SummonButton.interactable = true;
    }

    public void SummonHero()
    {
        if (UncommonChance >= Random.Range(0, 100 + UncommonChance))
        {
            roll = Random.Range(0, HLib.UncommonHeroes.Length);
            CastleScript.CollectUncommonHero(roll);
            GameObject summon = Instantiate(SummonPrefab, SummonSpot.position, SummonSpot.rotation);
            SummonedScript = summon.GetComponent(typeof(Summoned)) as Summoned;
            SummonedScript.SetHero(roll, 1);
            UncommonChance = 5;
        }
        else
        {
            roll = Random.Range(0, HLib.CommonHeroes.Length);
            CastleScript.CollectCommonHero(roll);
            GameObject summon = Instantiate(SummonPrefab, SummonSpot.position, SummonSpot.rotation);
            SummonedScript = summon.GetComponent(typeof(Summoned)) as Summoned;
            SummonedScript.SetHero(roll, 0);
            UncommonChance += (UncommonChance + 2) / 4;
        }
        SpendMana(75f);
    }

    public void SpendMana(float amount)
    {
        mana -= amount;
        ManaValue.text = mana.ToString("0") + "/" + maxMana.ToString("0");
        ManaBarFill.fillAmount = mana / maxMana;
        if (mana < 75f)
            SummonButton.interactable = false;
    }

    public void GainExp(int amount)
    {
        experience += amount;
        if (experience >= experienceRequired)
            LevelUp();
        ExperienceBarFill.fillAmount = (experience * 1f) / (experienceRequired * 1f);
    }

    void LevelUp()
    {
        level++;
        LevelValue.text = level.ToString("0");
        experience -= experienceRequired;
        maxMana += 40;
        manaRegen += 0.1f;
        manaPerClick += 0.01f;
        experienceRequired = NextLevelExp();
    }

    int NextLevelExp()
    {
        return 3 + (level * (level + 6)) / 6;
    }

    public void ManaOrb()
    {
        GainMana(30f + maxMana * 0.02f, true);
    }
}
