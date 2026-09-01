using UnityEngine;

public class Replayable : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public PeopleControllers peopleControllers;
    public StaminaBarUI staminaBarUI;
    public MinigamesControllers minigamesControllers;
    [SerializeField] private IsWinner isWinner;

    void Start()
    {
        AudioSystem.Instance?.Play(AudioType.BGM);
    }

    void Update()
    {
        
    }

    public void Replay()
    {
        win.SetActive(false);
        lose.SetActive(false);
        minigamesControllers.LastStand = true;
        peopleControllers.topPeople = peopleControllers.maxPeople/2;
        peopleControllers.bottomPeople = peopleControllers.maxPeople/2;
        staminaBarUI.bar.value = staminaBarUI.currentStamina / staminaBarUI.maxStamina;
        staminaBarUI.currentStaminaDrain = peopleControllers.topPeople * 0.002f;
        isWinner.GameOver = false;
        staminaBarUI.currentStamina = 50;

        AudioSystem.Instance?.Play(AudioType.BGM);
    }
}
