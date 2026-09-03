using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Replayable : MonoBehaviour
{
    public Slider Replaybar;
    private float maxbar;
    private float currentbar;
    public PeopleControllers peopleControllers;
    public StaminaBarUI staminaBarUI;
    public MinigamesControllers minigamesControllers;
    [SerializeField] private IsWinner isWinner;
    private float holdTimer = 0f;
    private float requiredHoldTime = 4f;

    void Start()
    {
        AudioSystem.Instance?.Play(AudioType.BGM);
        maxbar = requiredHoldTime;
        currentbar = 0f;
        Replaybar.value = currentbar / maxbar;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed && isWinner.GameOver == true)
        {
            Debug.Log("Space key is being held down.");
            holdTimer += Time.deltaTime;
            currentbar = holdTimer;
            Replaybar.value = currentbar / maxbar;
            if (holdTimer >= requiredHoldTime)
            {
                Replay();
                holdTimer = 0f;
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }

    public void Replay()
    {
        isWinner.win.SetActive(false);
        isWinner.lose.SetActive(false);
        isWinner.replay.SetActive(false);
        isWinner.startScreen.SetActive(true);
        isWinner.gameStarted = false;
        isWinner.remaining.SetActive(false);
        staminaBarUI.gameObject.SetActive(false);
        minigamesControllers.LastStand = true;
        peopleControllers.topPeople = peopleControllers.maxPeople/2;
        peopleControllers.bottomPeople = peopleControllers.maxPeople/2;
        staminaBarUI.bar.value = staminaBarUI.currentStamina / staminaBarUI.maxStamina;
        staminaBarUI.currentStaminaDrain = peopleControllers.topPeople * 0.002f;
        staminaBarUI.pointerController.moveSpeed = staminaBarUI.pointerController.baseSpeed;
        isWinner.GameOver = false;
        staminaBarUI.currentStamina = 50;

        AudioSystem.Instance?.Play(AudioType.BGM);
    }
}
