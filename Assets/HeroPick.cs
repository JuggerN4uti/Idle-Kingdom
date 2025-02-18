using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPick : MonoBehaviour
{
    public HeroesLibrary HLib;
    public Castle CastleScript;

    public GameObject PickHud;
    public Image[] PickImage;
    public TMPro.TextMeshProUGUI[] HpValue, ArValue, AdValue, AsValue;

    public int[] roll;

    void Start()
    {
        PickHud.SetActive(true);

        roll[0] = Random.Range(0, HLib.CommonHeroes.Length);

        do
        {
            roll[1] = Random.Range(0, HLib.CommonHeroes.Length);
        } while (roll[0] == roll[1]);

        do
        {
            roll[2] = Random.Range(0, HLib.CommonHeroes.Length);
        } while (roll[0] == roll[2] || roll[1] == roll[2]);

        for (int i = 0; i < 3; i++)
        {
            PickImage[i].sprite = HLib.CommonHeroes[roll[i]].UnitSprite;
            HpValue[i].text = HLib.CommonHeroes[roll[i]].HP.ToString("0");
            ArValue[i].text = HLib.CommonHeroes[roll[i]].AR.ToString("0");
            AdValue[i].text = HLib.CommonHeroes[roll[i]].AD.ToString("0");
            AsValue[i].text = HLib.CommonHeroes[roll[i]].AS.ToString("0.000");
        }
    }

    public void ChooseHero(int which)
    {
        CastleScript.CommonHeroesCollected[roll[which]]++;
        PickHud.SetActive(false);
    }
}
