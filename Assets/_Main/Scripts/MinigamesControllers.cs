using UnityEngine;

public class MinigamesControllers : MonoBehaviour
{
    public GameObject miniG1;
    public GameObject miniG2;
    private bool LastStand = true;
    public StaminaBarUI staminaBarUI;
    public PeopleControllers peopleControllers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        miniGame();
    }
    public void miniGame()
    {
        if (staminaBarUI.currentStamina >= 85)
        {
            miniG1.SetActive(false);
            miniG2.SetActive(true);
        }
        else if (staminaBarUI.currentStamina <= 15 && peopleControllers.topPeople == 1 && LastStand == true)
        {
            miniG1.SetActive(false);
            miniG2.SetActive(true);
            LastStand = false;
        }
        else
        {
            miniG1.SetActive(true);
            miniG2.SetActive(false);
        }
    }
}
