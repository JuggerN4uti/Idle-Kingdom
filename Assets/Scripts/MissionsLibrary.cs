using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsLibrary : MonoBehaviour
{
    public bool[] viableMission;
    public string[] missionText;
    public int[] missionBaseRequirement, missionDifficultyScale;

    int roll;

    public int RollMission()
    {
        do
        {
            roll = Random.Range(0, viableMission.Length);
        } while (!viableMission[roll]);
        return roll;
    }

    public void UnlockMission(int which)
    {
        viableMission[which] = true;
    }
}
