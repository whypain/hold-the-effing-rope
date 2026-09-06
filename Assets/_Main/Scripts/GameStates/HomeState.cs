using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeState : GameState
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject staminaBarUI;

    public override void Enter()
    {
        startScreen.SetActive(true);
        staminaBarUI.SetActive(false);

        AudioSystem.Instance?.Play(AudioType.BGM);
    }

    public override void Exit()
    {
        startScreen.SetActive(false); 
        staminaBarUI.SetActive(true);
    }

    public override void Tick(float deltaTime, GameStateManager manager)
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            manager.TransitionToState(EGameState.SkillCheck); 
            manager.GS.Initialize();
        }
    }
}