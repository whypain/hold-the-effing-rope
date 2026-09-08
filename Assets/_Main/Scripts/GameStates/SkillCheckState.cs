using System.Threading;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillCheckState : GameState
{
    [SerializeField] private GameObject minigame;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform pointerTransform;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float endMargin = 50f;

    [SerializeField] private RectTransform safeZone;
    [SerializeField] private RectTransform greatZone;
    [SerializeField] private RectTransform perfectZone;

    [Header("Camera Shake Settings")]
    [SerializeField] private float cameraShakeDuration = 0.5f;  
    [SerializeField] private float cameraShakeMagnitude = 1f;

    private Transform target;
    private float randomYMin;
    private float randomYMax;

    private bool isGracePeriod = true;
    private float gracePeriodDuration = 1.5f;
    private float gracePeriodTimer = 0f;

    private CancellationTokenSource graceAnimCts;

    public override void Enter() 
    { 
        minigame.SetActive(true); 

        isGracePeriod = true;

        graceAnimCts?.Cancel();
        graceAnimCts = new CancellationTokenSource();

        Sequence.Create()
            .Chain(Tween.Alpha(canvasGroup, 0f, 1f, 0.5f))
            .Chain(Tween.Alpha(canvasGroup, 1f, 0f, 0.5f, Ease.InQuad))
            .SetCancellationToken(graceAnimCts.Token)
            .SetRemainingCycles(-1);
    }

    public override void Exit() { minigame.SetActive(false); }

    public override void Tick(float deltaTime, GameStateManager manager)
    {
        if (isGracePeriod)
        {
            gracePeriodTimer += deltaTime;
            if (gracePeriodTimer >= gracePeriodDuration)
            {
                isGracePeriod = false;
                gracePeriodTimer = 0f;
                graceAnimCts?.Cancel();
                canvasGroup.alpha = 1f;
            }
        }

        UpdatePointerPosition(deltaTime, manager);

        // Check for input
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckSuccess();
        }

        if (manager.GS.stamina.currentStamina >= manager.GS.spamEnterThreshold)
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
            randomYMin = pointA.position.y - endMargin * canvas.scaleFactor;
            randomYMax = pointB.position.y + endMargin * canvas.scaleFactor;
            newPositionY.y = Random.Range(randomYMin, randomYMax);
            safeZone.transform.position = newPositionY;
            if (RectTransformUtility.RectangleContainsScreenPoint(perfectZone, pointerTransform.position, null))
            {
                Debug.Log("Perfect!");
                GlobalState.Instance.stamina.Refill(30f);
                AudioSystem.Instance?.Play(AudioType.SkillCheckPerfect);
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(greatZone, pointerTransform.position, null))
            {
                Debug.Log("Great!");
                GlobalState.Instance.stamina.Refill(20f);
                AudioSystem.Instance?.Play(AudioType.SkillCheckGreat);
            }
            else
            {
                Debug.Log("Success!");
                GlobalState.Instance.stamina.Refill(10f);
                AudioSystem.Instance?.Play(AudioType.SkillCheckGood);
            }

            if (isGracePeriod)
            {
                // exit grace period immediately after a successful skill check
                gracePeriodTimer = gracePeriodDuration;
            }
        }
        else
        {
            if (isGracePeriod) return;
            Debug.Log("Fail!");
            GlobalState.Instance.stamina.Drain(10f);
            AudioSystem.Instance?.Play(AudioType.SkillCheckMiss);
            CameraManager.Instance?.ShakeCamera(cameraShakeDuration, cameraShakeMagnitude);
        }
    }
}
