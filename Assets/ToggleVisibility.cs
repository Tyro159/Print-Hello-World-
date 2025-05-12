using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject targetObject;

    public void Toggle()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
        else
        {
            Debug.LogWarning("ToggleVisibility: No target object assigned!");
        }
    }
}
