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
    public Image[] MissionBackground;

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
        MissionBackground[which].color = new Color(0.345f, 0.325f, 0.451f, 1f);
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
        if (MissionProgress[I] >= MissionRequirement[I])
        {
            MissionProgressText[I].text = MissionRequirement[I].ToString("0") + "/" + MissionRequirement[I].ToString("0");
            MissionCompleted[I] = true;
            MissionBackground[I].color = new Color(0.259f, 0.541f, 0.302f, 1f);
        }
    }
}
