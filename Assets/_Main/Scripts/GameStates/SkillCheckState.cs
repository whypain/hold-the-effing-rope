using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillCheckState : GameState
{
    [SerializeField] private GameObject minigame;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform pointerTransform;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float endMargin = 50f;

    [SerializeField] private RectTransform safeZone;
    [SerializeField] private RectTransform greatZone;
    [SerializeField] private RectTransform perfectZone;

    private Transform target;
    private float randomYMin;
    private float randomYMax;

    public override void Enter() 
    { 
        minigame.SetActive(true); 
        randomYMin = pointA.position.y - endMargin * canvas.scaleFactor;
        randomYMax = pointB.position.y + endMargin * canvas.scaleFactor;
    }

    public override void Exit() { minigame.SetActive(false); }

    public override void Tick(float deltaTime, GameStateManager manager)
    {
        UpdatePointerPosition(deltaTime, manager);

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

    private void UpdatePointerPosition(float deltaTime, GameStateManager manager)
    {
        var gs = manager.GS;
        float speed = gs.skillCheckSpeed;

        if (target == null)
        {
            target = pointB;
        }

        // Change direction if the pointer reaches one of the points
        if (Vector3.Distance(pointerTransform.position, pointA.position) < 1f)
        {
            target = pointB;
        }
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 1f)
        {
            target = pointA;
        }

        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, target.position, speed * deltaTime * canvas.scaleFactor);
    }

    void CheckSuccess()
    {
        // Check if the pointer is within the safe zone
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            Vector2 newPositionY = safeZone.transform.position;
            newPositionY.y = Random.Range(randomYMin, randomYMax);
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
