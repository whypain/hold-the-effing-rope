using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    public Slider bar;
    public float maxStamina;
    public float currentStamina;
    public float staminaDrainRate;
    public float currentStaminaDrain;
    public PeopleControllers peopleControllers;
    public PointerController pointerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bar.value = currentStamina/ maxStamina;
        currentStaminaDrain = peopleControllers.topPeople*0.002f;
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
        currentStamina = currentStamina - currentStaminaDrain;
        bar.value = currentStamina / maxStamina;
    }
    public void ProcessResult()
    {
        if (currentStamina >= 100)
        {
            currentStamina = 50;
            peopleControllers.topPeople += 1;
            peopleControllers.bottomPeople -= 1;
            if (currentStaminaDrain > 0)
            {
                currentStaminaDrain -= staminaDrainRate;
                if (pointerController.moveSpeed > pointerController.baseSpeed)
                {
                    pointerController.moveSpeed -= pointerController.speedChangeRate;
                }
                if (currentStaminaDrain < 0)
                {
                    currentStaminaDrain = 0;
                }
            }
            bar.value = currentStamina / maxStamina;
        }
        if (currentStamina <= 0)
        {
            peopleControllers.bottomPeople += 1;
            peopleControllers.topPeople -= 1;
            currentStamina = 50;
            currentStaminaDrain += staminaDrainRate;
            pointerController.moveSpeed += pointerController.speedChangeRate;
            bar.value = currentStamina / maxStamina;
        }
    }
}
