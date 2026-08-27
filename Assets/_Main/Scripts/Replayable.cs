using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Replayable : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public PeopleControllers peopleControllers;
    public StaminaBarUI staminaBarUI;
    public MinigamesControllers minigamesControllers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
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
    }
}
