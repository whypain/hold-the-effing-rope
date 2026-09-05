using UnityEngine;
using UnityEngine.InputSystem;

public class SkillCheckState : GameState
{
    [SerializeField] private GameObject minigame;
    [SerializeField] private Transform pointerTransform;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [SerializeField] private RectTransform safeZone;
    [SerializeField] private RectTransform greatZone;
    [SerializeField] private RectTransform perfectZone;

    public override void Enter() { minigame.SetActive(true); }
    public override void Exit() { minigame.SetActive(false); }

    public override void Tick(float deltaTime, GameStateManager manager)
    {
        // Move the pointer towards the target position
        pointerTransform.position = Vector3.Lerp(
            pointA.position, pointB.position, Mathf.PingPong(Time.time * GlobalState.Instance.skillCheckSpeed, 1f)
        );

        // Check for input
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckSuccess();
        }

        if (manager.GS.stamina.currentStamina >= 85)
        {
            manager.TransitionToState(EGameState.Spam);
        }

        if (manager.GS.stamina.currentStamina <= 15 && manager.GS.topPeople == 1 && manager.GS.lastStand == LastStand.Enabled)
        {
            manager.GS.lastStand = LastStand.Activated;
            manager.TransitionToState(EGameState.Spam);
        }
    }

    public void SpeedUp()
    {
        var gs = GlobalState.Instance;
        gs.skillCheckSpeed = Mathf.Clamp(gs.skillCheckSpeed + gs.skillCheckSpeedChangeRate, 0f, gs.skillCheckMaxSpeed);
    }

    void CheckSuccess()
    {
        // Check if the pointer is within the safe zone
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            float randomY = Random.Range(pointA.position.y-50, pointB.position.y+50);
            Vector2 newPositionY = safeZone.transform.position;
            newPositionY.y = randomY;
            safeZone.transform.position = newPositionY;
            if (RectTransformUtility.RectangleContainsScreenPoint(perfectZone, pointerTransform.position, null))
            {
                Debug.Log("Perfect!");
                GlobalState.Instance.stamina.Refill(30f);
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(greatZone, pointerTransform.position, null))
            {
                Debug.Log("Great!");
                GlobalState.Instance.stamina.Refill(20f);
            }
            else
            {
                Debug.Log("Success!");
                GlobalState.Instance.stamina.Refill(10f);
            }
        }
        else
        {
            Debug.Log("Fail!");
            GlobalState.Instance.stamina.Drain(10f);
        }
    }
}
