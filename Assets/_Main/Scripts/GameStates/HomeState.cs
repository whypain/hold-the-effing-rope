using UnityEngine;
using UnityEngine.InputSystem;

public class HomeState : GameState
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject staminaBarUI;

    [SerializeField] private CanvasGroupFade blackScreen;
    [SerializeField] private CanvasGroupBlink blinkingText;
    [SerializeField] private SplashArt titleSplash;

    public override async void Enter()
    {
        startScreen.SetActive(true);
        staminaBarUI.SetActive(false);

        AudioSystem.Instance?.Play(AudioType.BGM);

        await blackScreen?.FadeOut();
        blinkingText?.Blink();

        titleSplash?.ShakeStay();
    }

    public override async void Exit()
    {
        blinkingText?.StopBlinking();

        await blackScreen?.FadeIn();

        startScreen.SetActive(false); 
        staminaBarUI.SetActive(true);

        await blackScreen?.FadeOut();
        titleSplash?.StopStayAnim();
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