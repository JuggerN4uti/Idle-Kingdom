using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour
{
    public Adventure AdventureScript;

    [Header("Stats")]
    public int ID, AD;
    public int[] gold;
    public float HitPoints, MaxHealth, AS, energy, bleed, bleedTick;
    bool active;

    [Header("UI")]
    public Image HealthBarFill, EnergyBarFill, EnemySprite;

    [Header("Pop Up")]
    public GameObject DamageTextObject;
    public Transform TextOrigin;
    public Rigidbody2D Body;
    DamageText DamageTextScript;

    public float lastDamage;

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
            if (bleed > 0f)
            {
                bleedTick -= Time.deltaTime;
                if (bleedTick <= 0f)
                    BleedTick();
            }
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
        bleed = 0f;
        bleedTick = 0.7f;
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

    public void TakeDamage(float amount, bool crit = false, float bleed = 0f)
    {
        //amount /= 1f + partyArmor * 0.01f;
        amount *= AdventureScript.damageMultiplyer;
        if (bleed > 0f)
            GainBleed(amount * bleed);
        //Debug.Log(amount);
        lastDamage = amount;
        PopUpText(amount, crit);
        HitPoints -= amount;
        HealthBarFill.fillAmount = HitPoints / MaxHealth;

        if (HitPoints <= 0f)
            Defeated();
    }

    public void GainBleed(float amount)
    {
        bleed += amount;
    }

    void BleedTick()
    {
        float amount = bleed * 0.75f;
        if (amount < 1f)
        {
            bleedTick += 0.75f / amount;
            amount = 1f;
        }
        else bleedTick += 0.75f;

        PopUpText(amount, false);
        HitPoints -= amount;
        HealthBarFill.fillAmount = HitPoints / MaxHealth;

        if (HitPoints <= 0f)
            Defeated();
    }

    void PopUpText(float value, bool crit = false)
    {
        //TextOrigin.position = new Vector3(transform.position.x + Random.Range(-0.25f, 0.25f), transform.position.y + Random.Range(-0.25f, 0.25f), 0f);
        TextOrigin.rotation = Quaternion.Euler(TextOrigin.rotation.x, TextOrigin.rotation.y, Body.rotation + Random.Range(-22f, 22f));
        GameObject text = Instantiate(DamageTextObject, TextOrigin.position, transform.rotation);
        Rigidbody2D text_body = text.GetComponent<Rigidbody2D>();
        DamageTextScript = text.GetComponent(typeof(DamageText)) as DamageText;
        DamageTextScript.SetText(value, crit);
        text_body.AddForce(TextOrigin.up * Random.Range(1.65f, 2.31f), ForceMode2D.Impulse);
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
