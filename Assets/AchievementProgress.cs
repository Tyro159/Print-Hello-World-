using System.IO;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AchievementProgress
{
    public string achievementName;
    public bool isUnlocked;
}

[System.Serializable]
public class AchievementProgressList
{
    public List<AchievementProgress> achievements = new List<AchievementProgress>();
}

