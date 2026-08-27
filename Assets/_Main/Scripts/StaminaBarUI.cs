using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    public Slider bar;
    public float maxStamina;
    public float currentStamina;
    public float staminaDrainRate;
    public PointerController pointerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bar.value = currentStamina/ maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStamina >= -11)
        {
            AutoDrainStamina();
        }
        if (currentStamina >= 100 || currentStamina <= 0)
        {
            ProcessResult();
        }
    }
    public void AutoDrainStamina()
    {
        if (currentStamina > 100)
        {
            currentStamina = 101;
        }
        currentStamina = currentStamina - staminaDrainRate;
        bar.value = currentStamina / maxStamina;
    }
    public void ProcessResult()
    {
        if (currentStamina >= 100)
        {
            currentStamina = 50;
            if (staminaDrainRate > 0)
            {
                staminaDrainRate -= 0.002f;
                if (pointerController.moveSpeed > 1000f)
                {
                    pointerController.moveSpeed -= 400f;
                }
                if (staminaDrainRate < 0)
                {
                    staminaDrainRate = 0;
                }
            }
            bar.value = currentStamina / maxStamina;
        }
        if (currentStamina <= 0)
        {
            currentStamina = 50;
            staminaDrainRate += 0.002f;
            pointerController.moveSpeed += 400f;
            bar.value = currentStamina / maxStamina;
        }
    }
}
