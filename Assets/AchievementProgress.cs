using System.IO;
using UnityEngine;

[System.Serializable]
public class AchievementData
{
    public string name;
    public bool isUnlocked;
}

public class AchievementSaveLoad : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/achievements.json";
        LoadAchievements();
    }

    public void SaveAchievements()
    {
        AchievementData[] data = AchievementManager.Instance.achievements
            .ConvertAll(a => new AchievementData { name = a.achievementName, isUnlocked = a.isUnlocked })
            .ToArray();

        File.WriteAllText(filePath, JsonUtility.ToJson(data));
    }

    public void LoadAchievements()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            AchievementData[] data = JsonUtility.FromJson<AchievementData[]>(json);

            foreach (var d in data)
            {
                Achievement achievement = AchievementManager.Instance.achievements.Find(a => a.achievementName == d.name);
                if (achievement != null) achievement.isUnlocked = d.isUnlocked;
            }
        }
    }
}
