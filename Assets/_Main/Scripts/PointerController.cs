using UnityEngine;
using UnityEngine.InputSystem;

public class PointerController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public float moveSpeed;
    public float baseSpeed;
    public float speedChangeRate;

    private float direction = 1f;
    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    public StaminaBarUI staminaBarUI;
    float randomX;

    void Start()
    {
        baseSpeed = moveSpeed;
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;
    }

    void Update()
    {
        // Move the pointer towards the target position
        pointerTransform.position = Vector3.Lerp(pointA.position, pointB.position, Mathf.PingPong(Time.time * moveSpeed, 1f));

        // Change direction if the pointer reaches one of the points
        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
            direction = 1f;
        }
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
            direction = -1f;
        }

        // Check for input
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckSuccess();
        }
    }

    void CheckSuccess()
    {
        // Check if the pointer is within the safe zone
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            Debug.Log("Success!");
            randomX = Random.Range(pointA.position.y+50, pointB.position.y-50);
            Vector2 newPositionX = safeZone.transform.position;
            newPositionX.y = randomX;
            safeZone.transform.position = newPositionX;
            if (staminaBarUI.currentStamina < staminaBarUI.maxStamina)
            staminaBarUI.currentStamina += 10f;
        }
        else
        {
            Debug.Log("Fail!");
            if (staminaBarUI.currentStamina > 1)
            staminaBarUI.currentStamina -= 10f;
        }
    }
}
