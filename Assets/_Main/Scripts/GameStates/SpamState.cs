using UnityEngine;
using UnityEngine.InputSystem;

public class SpamState : GameState
{
    [SerializeField] private GameObject minigame;

    public override void Enter() { minigame.SetActive(true); }
    public override void Exit() { minigame.SetActive(false); }

    public override void Tick(float deltaTime, GameStateManager manager)
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Spam");
            float refillAmount = GlobalState.Instance.lastStand == LastStand.Activated
                ? GlobalState.Instance.lastStandStaminaGainAmount
                : GlobalState.Instance.spamStaminaGainAmount;

            GlobalState.Instance.stamina.Refill(refillAmount);
        }

        if (manager.GS.stamina.currentStamina < 85 && manager.GS.lastStand != LastStand.Activated)
        {
            manager.TransitionToState(EGameState.SkillCheck);
        }

        if (manager.GS.stamina.currentStamina >= 30 && manager.GS.lastStand == LastStand.Activated)
        {
            manager.GS.lastStand = LastStand.Used;
            manager.TransitionToState(EGameState.SkillCheck);
        }
    }
}
