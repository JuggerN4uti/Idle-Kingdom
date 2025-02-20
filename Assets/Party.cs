using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour
{
    public Castle CastleScript;
    public HeroesLibrary HLib;
    public Adventure AdventureScript;
    public Hero[] PartyHeroes;
    public int PartyLimit, PartyCount, heroesChoices, partyHP, partyAR;
    public bool[] HeroesInParty;

    public GameObject PartyHud;
    public Image[] PartyBackground, PartyPortraitIcon, HeroImage;
    public GameObject[] PartyPortraitObject, HeroObject;
    public Button[] HeroButton;
    public Button StartButton;
    public int[] HeroID, PartyID;
    public TMPro.TextMeshProUGUI HpValue, ArValue;

    public void Open()
    {
        PartyHud.SetActive(true);
        StartButton.interactable = false;

        for (int i = 0; i < 6; i++)
        {
            PartyBackground[i].color = new Color(0f, 0f, 0f, 1f);
            PartyPortraitObject[i].SetActive(false);
        }

        for (int i = 0; i < PartyLimit; i++)
        {
            PartyBackground[i].color = new Color(1f, 1f, 1f, 1f);
        }

        DisplayHeroesCollected();
    }

    void DisplayHeroesCollected()
    {
        PartyCount = 0; heroesChoices = 0; partyHP = 0; partyAR = 0;

        for (int i = 0; i < HeroesInParty.Length; i++)
        {
            HeroesInParty[i] = false;
        }

        for (int i = 0; i < HeroObject.Length; i++)
        {
            HeroObject[i].SetActive(false);
            HeroButton[i].interactable = true;
        }

        for (int i = 0; i < CastleScript.CommonHeroesCollected.Length; i++)
        {
            if (CastleScript.CommonHeroesCollected[i] > 0)
            {
                HeroObject[heroesChoices].SetActive(true);
                HeroImage[heroesChoices].sprite = HLib.CommonHeroes[i].UnitPortrait;
                HeroID[heroesChoices] = i;
                heroesChoices++;
            }
        }
    }

    public void SelectHero(int which)
    {
        HeroesInParty[HeroID[which]] = true;
        PartyID[PartyCount] = HeroID[which];
        PartyHeroes[PartyCount] = HLib.CommonHeroes[HeroID[which]];
        partyHP += PartyHeroes[PartyCount].HP;
        HpValue.text = partyHP.ToString("0");
        partyAR += PartyHeroes[PartyCount].AR;
        ArValue.text = partyAR.ToString("0");
        PartyCount++;

        DisplayParty();
    }

    void DisplayParty()
    {
        for (int i = 0; i < PartyCount; i++)
        {
            PartyPortraitObject[i].SetActive(true);
            PartyPortraitIcon[i].sprite = PartyHeroes[i].UnitPortrait;
        }

        CheckButtons();
    }

    void CheckButtons()
    {
        if (PartyCount >= PartyLimit)
        {
            for (int i = 0; i < heroesChoices; i++)
            {
                HeroButton[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < heroesChoices; i++)
            {
                if (HeroesInParty[HeroID[i]])
                    HeroButton[i].interactable = false;
                else HeroButton[i].interactable = true;
            }
        }

        if (PartyCount > 0)
            StartButton.interactable = true;
        else StartButton.interactable = false;
    }

    public void RemoveHero(int which)
    {
        HeroesInParty[PartyID[which]] = false;
        partyHP -= PartyHeroes[which].HP;
        HpValue.text = partyHP.ToString("0");
        partyAR -= PartyHeroes[which].AR;
        ArValue.text = partyAR.ToString("0");

        if (which == PartyCount - 1)
            PartyPortraitObject[which].SetActive(false);
        else
        {
            for (int i = which; i < PartyCount - 1; i++)
            {
                PartyID[i] = PartyID[i + 1];
                PartyHeroes[i] = PartyHeroes[i + 1];
            }
            PartyPortraitObject[PartyCount - 1].SetActive(false);
        }
        PartyCount--;
        DisplayParty();
    }

    public void StartAdventure()
    {
        PartyHud.SetActive(false);
        AdventureScript.SetAdventure(0);
    }
}
