using System.Collections.Generic;
using UnityEngine;

public class AchievementMenu : MonoBehaviour
{
    public GameObject achievementPrefab; // Prefab for each achievement UI entry
    public Transform achievementListContainer; // Parent container for achievements

    private void OnEnable() // Runs every time the menu is opened
    {
        RefreshAchievements();
    }

    void RefreshAchievements()
    {
        // Clear existing UI elements before reloading
        foreach (Transform child in achievementListContainer)
        {
            Destroy(child.gameObject);
        }

        List<Achievement> achievements = AchievementManager.Instance.achievements;

        foreach (Achievement achievement in achievements)
        {
            GameObject newAchievement = Instantiate(achievementPrefab, achievementListContainer);
            AchievementUIEntry achievementUI = newAchievement.GetComponent<AchievementUIEntry>();

            if (achievementUI != null)
            {
                achievementUI.Setup(achievement);
            }
        }
    }
}
