using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUIEntry : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private Color lockedColor = new Color(1, 1, 1, 0.3f); // Grayscale effect for locked
    private Color unlockedColor = Color.white; // Normal color for unlocked

    public void Setup(Achievement achievement)
    {
        titleText.text = achievement.achievementName;
        descriptionText.text = achievement.description;

        if (achievement.isUnlocked)
        {
            icon.sprite = achievement.icon;
            icon.color = unlockedColor;
            titleText.alpha = 1f;
            descriptionText.alpha = 1f;
        }
        else
        {
            icon.sprite = achievement.icon;
            icon.color = lockedColor; // Dim icon
            titleText.alpha = 0.5f;
            descriptionText.alpha = 0.5f;
        }
    }
}
