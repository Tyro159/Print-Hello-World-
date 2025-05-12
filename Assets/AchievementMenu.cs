using System.Collections.Generic;
using UnityEngine;

public class AchievementMenu : MonoBehaviour
{
    public GameObject achievementPrefab;
    public Transform achievementListContainer;

    private void OnEnable() // Run every time menu is loaded
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
