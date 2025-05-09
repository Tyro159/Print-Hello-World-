using System.IO;
using UnityEngine;
using System.Collections.Generic;

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
            Destroy(gameObject);
        }

        filePath = Application.persistentDataPath + "/achievements.json";
        Debug.Log("Achievement save path: " + filePath);
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
            SaveAchievements();
            return;
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
