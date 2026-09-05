using UnityEngine;

public enum LastStand
{
    Disabled,
    Enabled,   // last stand can be activated when stamina is low
    Activated, // last stand is currently active
    Used,
}

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
    public float staminaDrain;
    public float staminaDrainMult;
    public float staminaDrainChangeRate;

    [Header("Configs")]
    [Header("Skill Check")]
    public float skillCheckSpeed;
    public float skillCheckBaseSpeed;
    public float skillCheckMaxSpeed;
    public float skillCheckSpeedChangeRate;

    [Header("Spam")]
    public LastStand lastStand;
    public float spamStaminaGainAmount;
    public float lastStandStaminaGainAmount;

    [Header("Debug")]
    public bool isInGame;

    private int m_topPeople;
    private int m_bottomPeople;
    private float m_staminaDrain;
    private float m_staminaDrainDecreaseRate;
    private float m_skillCheckSpeed;
    private LastStand m_lastStand;

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

    public void Initialize()
    {
        stamina = new StaminaController(maxStamina, startingStamina);
        staminaDrain = topPeople;

        // Store the initial values for reset
        m_topPeople = topPeople;
        m_bottomPeople = bottomPeople;
        m_staminaDrain = staminaDrain;
        m_staminaDrainDecreaseRate = staminaDrainChangeRate;
        m_skillCheckSpeed = skillCheckSpeed;
        m_lastStand = lastStand;
    }

    public void Reset()
    {
        topPeople = m_topPeople;
        bottomPeople = m_bottomPeople;
        staminaDrain = m_staminaDrain;
        staminaDrainChangeRate = m_staminaDrainDecreaseRate;
        skillCheckSpeed = m_skillCheckSpeed;
        lastStand = m_lastStand;

        stamina.Set(startingStamina);
    }
}
