using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
    public MissionsLibrary MLib;
    public Castle CastleScript;
    public Wizard WizardScript;

    public int MissionsCount;
    public GameObject[] MissionWindow;
    public bool[] MissionActive;

    [Header("Missions")]
    public int[] MissionID;
    public int[] MissionProgress, MissionRequirement;
    public bool[] MissionCompleted;
    public TMPro.TextMeshProUGUI[] MissionText, MissionProgressText;
    public Image[] MissionProgressFill, ExpRewardImage, BonusRewardImage;
    public Sprite[] ExpOrbsSprite, BonusDropSprite;
    public Button[] MissionButton;
    public GameObject[] MissionBonusObject;
    int rewardsCount;

    [Header("Missions Stats")]
    public int missionsCompleted;
    public int[] MissionDifficulty, MissionXP, MissionBonus;
    public int instantPercent;

    public void NewMissionSlot()
    {
        MissionWindow[MissionsCount].SetActive(true);
        SetMission(MissionsCount);
        MissionsCount++;
    }

    void SetMission(int which)
    {
        MissionID[which] = MLib.RollMission();
        SetMissionRewards(which);
        MissionCompleted[which] = false;
        MissionButton[which].interactable = false;
        MissionActive[MissionID[which]] = true;
        MissionText[which].text = MLib.missionText[MissionID[which]];
        MissionRequirement[which] = MLib.missionBaseRequirement[MissionID[which]] + (MLib.missionDifficultyScale[MissionID[which]] * MissionDifficulty[which]) / 10;
        MissionProgress[which] = (MissionRequirement[which] * instantPercent) / 100;
        MissionProgressText[which].text = MissionProgress[which].ToString("0") + "/" + MissionRequirement[which].ToString("0");
        MissionProgressFill[which].fillAmount = (MissionProgress[which] * 1f) / (MissionRequirement[which] * 1f);
    }

    public void ProgressMissionID(int ID, int count)
    {
        for (int i = 0; i < MissionsCount; i++)
        {
            if (MissionID[i] == ID && !MissionCompleted[i])
                ProgressMission(i, count);
        }
    }

    void ProgressMission(int I, int amount)
    {
        MissionProgress[I] += amount;
        MissionProgressText[I].text = MissionProgress[I].ToString("0") + "/" + MissionRequirement[I].ToString("0");
        MissionProgressFill[I].fillAmount = (MissionProgress[I] * 1f) / (MissionRequirement[I] * 1f);
        if (MissionProgress[I] >= MissionRequirement[I])
        {
            MissionProgressText[I].text = MissionRequirement[I].ToString("0") + "/" + MissionRequirement[I].ToString("0");
            MissionCompleted[I] = true;
            MissionButton[I].interactable = true;
            MissionProgressFill[I].fillAmount = 1f;
        }
    }

    public void CompleteMission(int which)
    {
        WizardScript.GainExp(MissionXP[which]);
        switch (MissionBonus[which])
        {
            case 1:
                WizardScript.ManaOrb();
                break;
        }
        //CastleScript.GetAChest();
        missionsCompleted++;
        SetMission(which);
    }

    void SetMissionRewards(int which)
    {
        MissionDifficulty[which] = 1 + WizardScript.level / 2;
        MissionXP[which] = SetMissionExp();
        ExpRewardImage[which].sprite = ExpOrbsSprite[MissionXP[which] - 1];
        MissionDifficulty[which] += MissionXP[which] + (MissionDifficulty[which] * MissionXP[which]) / 3;
        MissionBonus[which] = SetMissionBonus();
        if (MissionBonus[which] == 0)
        {
            MissionBonusObject[which].SetActive(false);
        }
        else
        {
            MissionBonusObject[which].SetActive(true);
            BonusRewardImage[which].sprite = BonusDropSprite[MissionBonus[which] - 1];
            MissionDifficulty[which] += MissionBonus[which] + (MissionDifficulty[which] * MissionBonus[which]) / 5;
        }
    }

    int SetMissionExp()
    {
        if (WizardScript.level == 1)
            return 1;
        else if (WizardScript.level < 6)
            return Random.Range(1, 3);
        else if (WizardScript.level < 18)
            return Random.Range(1, 4);
        else return Random.Range(2, 4);
    }

    int SetMissionBonus()
    {
        if (WizardScript.level >= Random.Range(0, 5 + WizardScript.level))
            return 1; // Mana
        else return 0; // Nothing
    }
}
