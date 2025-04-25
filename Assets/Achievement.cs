using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    public string achievementName;
    public string description;
    public Sprite icon;
    public bool isUnlocked;
}
