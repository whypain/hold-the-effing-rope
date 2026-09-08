using UnityEngine;
using UnityEngine.UI;

public class GameOverState : GameState
{
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject replay;


    [Header("Replay")]
    [SerializeField] private Image replayBar;
    [SerializeField] private float currentbar;
    [SerializeField] private float holdToReplaySeconds = 4f;

    [Header("Win/Lose Screen")]
    [SerializeField] private CanvasGroupFade blackScreen;
    [SerializeField] private SplashArt winSplash;
    [SerializeField] private SplashArt loseSplash;

    private float holdTimer = 0f;

    public override void Enter()
    {
        var gs = GlobalState.Instance;
        if (gs == null) return;

        replay.SetActive(true);
        AudioSystem.Instance?.StopRopeAmbient();

        if (gs.topPeople == gs.maxPeople)         OnWin();
        else if (gs.bottomPeople == gs.maxPeople) OnLose();
    }

    public override void Exit()
    {
        win.SetActive(false);
        lose.SetActive(false);
        replay.SetActive(false);

        winSplash?.StopStayAnim();
        loseSplash?.StopStayAnim();
    }

    private async void OnWin()
    {
        win.SetActive(true);
        lose.SetActive(false);
        AudioSystem.Instance?.Play(AudioType.Win);

        await winSplash?.BounceIn();
        winSplash?.BounceStay();
        winSplash?.ShakeStay();
    }

    private async void OnLose()
    {
        lose.SetActive(true);
        win.SetActive(false);
        AudioSystem.Instance?.Play(AudioType.Lose);

        await loseSplash?.BounceIn();
        loseSplash?.BounceStay();
        loseSplash?.ShakeStay();
    }

    public override void Tick(float deltaTime, GameStateManager manager)
    {
        if (InputManager.Instance.PlayerAction.action.IsPressed())
        {
            Debug.Log("Space key is being held down.");
            holdTimer += deltaTime;
            if (holdTimer >= holdToReplaySeconds)
            {
                holdTimer = 0f;

                manager.GS.Reset();
                manager.TransitionToState(EGameState.Home);
            }
        }
        else
        {
            holdTimer = Mathf.Max(0, holdTimer - deltaTime);
        }
        replayBar.fillAmount = holdTimer / holdToReplaySeconds;
    }
}