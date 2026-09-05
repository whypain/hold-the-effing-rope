using UnityEngine;

[System.Serializable]
public class StaminaController
{
    public float currentStamina { get; private set; }
    public float maxStamina { get; private set; }

    public StaminaController(float maxStamina, float startingStamina)
    {
        this.maxStamina = maxStamina;
        this.currentStamina = startingStamina;
    }

    public void Set(float stamina)
    {
        currentStamina = Mathf.Clamp(stamina, 0, maxStamina);
    }

    public void Drain(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
    }

    public void Refill(float amount)
    {
        currentStamina += amount;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }
}
