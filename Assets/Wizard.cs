using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wizard : MonoBehaviour
{
    [Header("Scripts")]
    public Castle CastleScript;

    [Header("Mana")]
    public float mana;
    public float maxMana, manaRegen, manaPerClick;

    [Header("Level")]
    public int level;
    public int experience, experienceRequired;

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
        CastleScript.CollectCommonHero(Random.Range(0, 4));
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
    }

    void LevelUp()
    {
        level++;
        experience -= experienceRequired;
        maxMana += 10;
        manaRegen += 0.1f;
        manaPerClick += 0.01f;
        experienceRequired = NextLevelExp();
    }

    int NextLevelExp()
    {
        return 5 + (level * (level + 7)) / 8;
    }
}
