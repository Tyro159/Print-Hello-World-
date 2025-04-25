using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CorePro.ButtonPro;

public class ChallengeButtonUnlocker : MonoBehaviour
{
    public List<ButtonPro> challengeButtons;

    private void OnEnable()
    {
        StartCoroutine(WaitForAchievementManager());
    }

    IEnumerator WaitForAchievementManager()
    {
        // Wait until AchievementManager.Instance is initialized
        while (AchievementManager.Instance == null)
            yield return null;

        Debug.Log("Updating Challenge Buttons.");
        UpdateChallengeButtons();
    }

    public void UpdateChallengeButtons()
    {
        if (challengeButtons.Count != 20)
        {
            Debug.LogError("ChallengeButtonUnlocker: Expected 20 buttons assigned!");
            return;
        }

        for (int i = 0; i < 20; i++)
        {
            int groupIndex = i / 5;
            bool previousGroupUnlocked = groupIndex == 0 || IsGroupCompleted(groupIndex - 1);

            if (previousGroupUnlocked)
            {
                challengeButtons[i].SetInteractable(true);
                Debug.Log($"Unlocking Challenge Button {i + 1}.");
            }
            else
            {
                challengeButtons[i].SetInteractable(false);
            }
        }
    }

    private bool IsGroupCompleted(int groupIndex)
    {
        int start = groupIndex * 5;
        int end = start + 5;

        for (int i = start; i < end; i++)
        {
            string achievementKey = $"PythonC{i + 1}";
            Achievement achievement = AchievementManager.Instance.achievements.Find(a => a.name == achievementKey);
            if (achievement == null || !achievement.isUnlocked)
                return false;
        }

        return true;
    }
}
