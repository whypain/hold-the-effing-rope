using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;
    [SerializeField] private Transform target;

    void Update()
    {
        if (GlobalState.Instance == null || GlobalState.Instance.stamina == null) return;

        var stamina = GlobalState.Instance.stamina;
        if (stamina.maxStamina <= 0) return;

        // Update the NPC's position based on the stamina bar's current value
        float staminaPercentage = stamina.currentStamina / stamina.maxStamina;
        target.position = Vector3.Lerp(bottom.position, top.position, staminaPercentage);
    }
}
