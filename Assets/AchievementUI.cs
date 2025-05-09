using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AchievementUI : MonoBehaviour
{
    public static AchievementUI Instance;
    public GameObject achievementPopup;
    public GameObject achievementSaveManager;
    public TextMeshProUGUI achievementText;
    public Image achievementIcon;
    public AudioSource mainTheme;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(achievementPopup);
            DontDestroyOnLoad(achievementSaveManager);
            DontDestroyOnLoad(mainTheme);
            Debug.Log("AchievementUI Instance set!");
        }
        else
        {
            Debug.LogError("Duplicate AchievementUI detected!");
            Destroy(gameObject);
            Destroy(achievementPopup);
            Destroy(achievementSaveManager);
            Destroy(mainTheme);
        }

        achievementPopup.SetActive(false);
    }
    
    

    public void ShowAchievement(Achievement achievement)
    {

        if (achievementPopup == null)
        {
            Debug.LogError("AchievementPopup is NULL! Trying to find it...");
            achievementPopup = GameObject.Find("AchievementPopup");
        }

        RectTransform popupRect = achievementPopup.GetComponent<RectTransform>();
        popupRect.anchoredPosition = new Vector2(0, 0);

        Debug.Log($"Setting up popup for: {achievement.achievementName}");
        achievementText.text = achievement.achievementName;
        achievementIcon.sprite = achievement.icon;
        Debug.Log($"Showing popup for: {achievement.achievementName}");
        achievementPopup.SetActive(true);

        // Hide the pop-up after 3 seconds
        Invoke(nameof(HideAchievementPopup), 3f);
        Debug.Log($"Hiding popup for: {achievement.achievementName}");
    }

    void HideAchievementPopup()
    {
        achievementPopup.SetActive(false);
    }
}
