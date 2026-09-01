using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;
    [SerializeField] private Transform target;
    [SerializeField] private StaminaBarUI staminaBar;

    void Update()
    {
        // Update the NPC's position based on the stamina bar's current value
        float staminaPercentage = staminaBar.currentStamina / staminaBar.maxStamina;
        target.position = Vector3.Lerp(bottom.position, top.position, staminaPercentage);
    }
}
