using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField] private Slider bar;

    [SerializeField] private Volume volume;
    [SerializeField] private Vignette vignette;

    [SerializeField] private float vignetteMaxIntensity = 0.6f;

    private GlobalState gs => GlobalState.Instance;

    private float currentStamina
    {
        get { return GlobalState.Instance.stamina.currentStamina; }
        set { GlobalState.Instance.stamina.Set(value); }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gs.stamina = new StaminaController(gs.maxStamina, gs.startingStamina);
        bar.value = currentStamina / gs.maxStamina;
        gs.currentStaminaDrain = gs.topPeople * 0.002f;

        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out vignette);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bar.value = currentStamina / gs.maxStamina;
        gameObject.SetActive(gs.isInGame);
    }

    private void FixedUpdate()
    {
        if (currentStamina >= -1)
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

        currentStamina -= gs.currentStaminaDrain * 10f;
        bar.value = currentStamina / gs.maxStamina;

        vignette.intensity.Override(Mathf.Clamp01(vignetteMaxIntensity - bar.value));
    }
}
