using UnityEngine;

public enum EGameState
{
    Home,
    SkillCheck,
    Spam,
    GameOver
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private HomeState homeState;
    [SerializeField] private SkillCheckState skillCheckState;
    [SerializeField] private SpamState spamState;
    [SerializeField] private GameOverState gameOverState;

    private GameState currentState;

    public GlobalState GS => GlobalState.Instance;

    void Start()
    {
        TransitionToState(homeState);
    }

    public void TransitionToState(EGameState newState)
    {
        switch (newState)
        {
            case EGameState.Home:
                TransitionToState(homeState);
                break;
            case EGameState.SkillCheck:
                TransitionToState(skillCheckState);
                break;
            case EGameState.Spam:
                TransitionToState(spamState);
                break;
            case EGameState.GameOver:
                TransitionToState(gameOverState);
                break;
        }
    }

    private void TransitionToState(GameState newState)
    {
        if (newState == currentState) return;

        Debug.Log($"Transitioning from {currentState?.GetType().Name ?? "None"} to {newState.GetType().Name}");

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    void Update()
    {
        currentState?.Tick(Time.deltaTime, this);

        GS.isInGame = !(currentState is HomeState || currentState is GameOverState);
        if (!GS.isInGame) return;

        float currStamina = GS.stamina.currentStamina;
        if (currStamina >= 100 || currStamina <= 0)
        {
            ProcessResult();
        }

        if (GS.topPeople <= 0 || GS.bottomPeople <= 0)
        {
            TransitionToState(gameOverState);
        }
    }

    private void ProcessResult()
    {
        float currStamina = GS.stamina.currentStamina;
        if (currStamina >= 100)
        {
            GS.stamina.Set(50);
            GS.topPeople += 1;
            GS.bottomPeople -= 1;
            if (GS.staminaDrain > 0)
            {
                GS.staminaDrain -= GS.staminaDrainChangeRate;
                skillCheckState.SpeedUp();
            }
        }
        if (currStamina <= 0)
        {
            GS.bottomPeople += 1;
            GS.topPeople -= 1;
            GS.stamina.Set(50);

            GS.staminaDrain += GS.staminaDrainChangeRate;
            skillCheckState.SpeedUp();
        }
    }
}
