using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpamSkillCheck : MonoBehaviour
{
    public RectTransform safeZone;    
    public RectTransform pointerTransform;
    public StaminaBarUI staminaBarUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckSuccess();
        }
    }
    void CheckSuccess()
    {
        // Check if the pointer is within the safe zone
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            Debug.Log("Success!"); 
            if (staminaBarUI.currentStamina < staminaBarUI.maxStamina)
                staminaBarUI.currentStamina += 1f;
        }
    }
}
