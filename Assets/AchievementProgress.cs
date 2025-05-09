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

public class AchievementSaveLoad : MonoBehaviour
{
    public static AchievementSaveLoad Instance;

    private string filePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // prevent duplicates
        }

        filePath = Application.persistentDataPath + "/achievements.json";
        LoadAchievements();
    }

    public void SaveAchievements()
    {
        AchievementProgressList saveData = new AchievementProgressList();

        foreach (var achievement in AchievementManager.Instance.achievements)
        {
            saveData.achievements.Add(new AchievementProgress
            {
                achievementName = achievement.achievementName,
                isUnlocked = achievement.isUnlocked
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Achievements saved to file.");
    }


    public void LoadAchievements()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("Achievements file not found. Creating new one...");

            SaveAchievements(); // Create a new default file
            return; // Exit early; the SaveAchievements call will handle everything
        }

        string json = File.ReadAllText(filePath);
        AchievementProgressList loadedProgress = JsonUtility.FromJson<AchievementProgressList>(json);

        foreach (var progress in loadedProgress.achievements)
        {
            var achievement = AchievementManager.Instance.achievements.Find(a => a.achievementName == progress.achievementName);
            if (achievement != null)
            {
                achievement.isUnlocked = progress.isUnlocked;
            }
        }

        Debug.Log("Achievements loaded from file.");
    }

}
