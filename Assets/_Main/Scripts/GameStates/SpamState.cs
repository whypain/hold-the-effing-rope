using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpamState : GameState
{
    [SerializeField] private GameObject minigame;
    [SerializeField] private RectTransform safeZone;    
    [SerializeField] private RectTransform pointerTransform;

    public override void Enter() { minigame.SetActive(true); }
    public override void Exit() { minigame.SetActive(false); }

    public override void Tick(float deltaTime, GameStateManager manager)
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckSuccess();
        }

        if (manager.GS.stamina.currentStamina < 85)
        {
            manager.TransitionToState(EGameState.SkillCheck);
        }
    }

    void CheckSuccess()
    {
        // Check if the pointer is within the safe zone
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            Debug.Log("Success!"); 
            GlobalState.Instance.stamina.Refill(GlobalState.Instance.spamStaminaGainAmount);
        }
    }
}
