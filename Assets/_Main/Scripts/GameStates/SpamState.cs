using UnityEngine;
using UnityEngine.InputSystem;

public class SpamState : GameState
{
    [SerializeField] private GameObject minigame;

    public async override void Enter()
    {
        minigame.SetActive(true);

        var  gs = GlobalState.Instance;
        if (gs == null) return;

        float staminaDrain = gs.staminaDrain;
        gs.staminaDrain = 0;
        await CameraManager.Instance?.ZoomIn();
        gs.staminaDrain = staminaDrain;

        AudioSystem.Instance?.PlayRopeAmbient();
    }

    public override void Exit() 
    {
        minigame.SetActive(false);
        CameraManager.Instance?.ZoomOut();

        AudioSystem.Instance?.StopRopeAmbient();
    }

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

        if (manager.GS.stamina.currentStamina < manager.GS.spamEnterThreshold && manager.GS.lastStand != LastStand.Activated)
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
