using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [SerializeField] public List<Achievement> achievements = new List<Achievement>();


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("AchievementManager Instance set!");
        }
        else
        {
            Debug.LogError("Duplicate AchievementManager detected!");
            Destroy(gameObject);
        }
    }


    public void UnlockAchievement(string name)
    {
        if (achievements == null || achievements.Count == 0)
        {
            Debug.LogError("Achievements list is EMPTY! Did you assign achievements in the Inspector?");
            return;
        }

        Debug.Log($"Searching for achievement: {name}");
        Achievement achievement = achievements.Find(a => a.achievementName == name);

        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true;

            // Show popup
            AchievementUI.Instance.ShowAchievement(achievement);
            
            // Save to JSON
            AchievementSaveLoad.Instance.SaveAchievements();
        }
        else
        {
            Debug.LogError($"Achievement '{name}' NOT FOUND in AchievementManager!");
        }
    }

}
