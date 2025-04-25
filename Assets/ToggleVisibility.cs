using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject targetObject; // Assign the object to toggle in Inspector

    public void Toggle()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf); // Switch state
        }
        else
        {
            Debug.LogWarning("ToggleVisibility: No target object assigned!");
        }
    }
}
