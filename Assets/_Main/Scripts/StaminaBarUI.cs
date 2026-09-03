using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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

    public Volume volume;
    public Vignette vignette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bar.value = currentStamina/ maxStamina;
        currentStaminaDrain = peopleControllers.topPeople*0.002f;

        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out vignette);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStamina >= 100 || currentStamina <= 0)
        {
            ProcessResult();
        }
    }

    private void FixedUpdate()
    {
        if (currentStamina >= -11)
        {
            AutoDrainStamina();
        }
    }

    public void AutoDrainStamina()
    {
        if (currentStamina > 100)
        {
            currentStamina = 101;
        }
        currentStamina = currentStamina - currentStaminaDrain * 10f;
        bar.value = currentStamina / maxStamina;

        vignette.intensity.Override(Mathf.Clamp01(0.8f - bar.value));
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
                if (pointerController.moveSpeed >= pointerController.maxSpeed)
                {
                    pointerController.moveSpeed = pointerController.maxSpeed;
                }
                else
                {
                    pointerController.moveSpeed += pointerController.speedChangeRate;
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
            if (pointerController.moveSpeed >= pointerController.maxSpeed)
            {
                pointerController.moveSpeed = pointerController.maxSpeed;
            }
            else
            {
                pointerController.moveSpeed += pointerController.speedChangeRate;
            }
            bar.value = currentStamina / maxStamina;
        }
    }
}
