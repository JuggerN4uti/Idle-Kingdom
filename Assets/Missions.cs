using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
    public MissionsLibrary MLib;

    public int MissionsCount;
    public GameObject[] MissionWindow;
    public bool[] MissionActive;

    [Header("Missions")]
    public int[] MissionID;
    public int[] MissionProgress, MissionRequirement;
    public bool[] MissionCompleted;
    public TMPro.TextMeshProUGUI[] MissionText, MissionProgressText;
    public Image[] MissionProgressFill;

    public void NewMissionSlot()
    {
        MissionWindow[MissionsCount].SetActive(true);
        SetMission(MissionsCount);
        MissionsCount++;
    }

    void SetMission(int which)
    {
        MissionID[which] = MLib.RollMission();
        MissionCompleted[which] = false;
        MissionProgressFill[which].fillAmount = 0f;
        MissionActive[MissionID[which]] = true;
        MissionText[which].text = MLib.missionText[MissionID[which]];
        MissionProgress[which] = 0;
        MissionRequirement[which] = MLib.missionBaseRequirement[MissionID[which]];
        MissionProgressText[which].text = MissionProgress[which].ToString("0") + "/" + MissionRequirement[which].ToString("0");
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
            MissionProgressFill[I].fillAmount = 1f;
        }
    }
}
