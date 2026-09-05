using UnityEngine;

public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance { get; private set; }

    [Header("People")]
    public int topPeople;
    public int bottomPeople;
    public int maxPeople;

    [Header("Stamina")]
    public StaminaController stamina;
    public float maxStamina;
    public float startingStamina;
    public float startingDrain;
    public float currentStaminaDrain;
    public float staminaDrainRate;

    [Header("Configs")]
    public bool lastStand;
    public bool modeLastStand;
    public bool lastStandMode;
    public float spamStaminaGainAmount;
    public float skillCheckSpeed;
    public float skillCheckBaseSpeed;

    [Header("Debug")]
    public bool isInGame;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
