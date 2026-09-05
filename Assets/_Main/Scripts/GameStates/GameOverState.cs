using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameOverState : GameState
{
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject replay;

    [SerializeField] private GameObject remaining;

    [Header("Replay")]
    [SerializeField] private Slider replayBar;
    [SerializeField] private float currentbar;
    [SerializeField] private float holdToReplaySeconds = 4f;

    private float holdTimer = 0f;

    public override void Enter()
    {
        var gs = GlobalState.Instance;
        if (gs == null) return;

        replay.SetActive(true);
        remaining.SetActive(false);

        if (gs.topPeople == gs.maxPeople)
        {
            win.SetActive(true);
            lose.SetActive(false);
            AudioSystem.Instance?.Play(AudioType.Win);
        }
        else if (gs.bottomPeople == gs.maxPeople)
        {
            lose.SetActive(true);
            win.SetActive(false);
            AudioSystem.Instance?.Play(AudioType.Lose);
        }
    }

    public override void Exit()
    {
        win.SetActive(false);
        lose.SetActive(false);
        replay.SetActive(false);
        remaining.SetActive(false);
    }

    public override void Tick(float deltaTime, GameStateManager manager)
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            Debug.Log("Space key is being held down.");
            holdTimer += deltaTime;
            replayBar.value = holdTimer / holdToReplaySeconds;
            if (holdTimer >= holdToReplaySeconds)
            {
                ResetGlobalState();
                holdTimer = 0f;

                manager.TransitionToState(EGameState.Home);
            }
        }
        else
        {
            holdTimer = Mathf.Max(0, holdTimer - deltaTime);
            replayBar.value = holdTimer / holdToReplaySeconds;
        }
    }

    private void ResetGlobalState()
    {
        var gs = GlobalState.Instance;
        if (gs == null) return;

        int maxPeople = gs.maxPeople;
        int topPeople = gs.topPeople;

        gs.lastStand = true;
        gs.topPeople = maxPeople/2;
        gs.bottomPeople = maxPeople/2;
        gs.currentStaminaDrain = topPeople * 0.002f;
        gs.skillCheckSpeed = gs.skillCheckBaseSpeed;
        gs.stamina.Set(50);

        AudioSystem.Instance?.Play(AudioType.BGM);
    }
}