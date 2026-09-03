using UnityEngine;

public class MinigamesControllers : MonoBehaviour
{
    public GameObject miniG1;
    public GameObject miniG2;
    public bool LastStand = true;
    public bool ModeLastStand = false;
    private bool LastStandMode = false;
    public SpamSkillCheck spamSkillCheck;
    public StaminaBarUI staminaBarUI;
    public PeopleControllers peopleControllers;
    public IsWinner isWinner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        miniG1.SetActive(false);
        miniG2.SetActive(false);
        staminaBarUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isWinner.gameStarted)
        {
            miniGame();
            staminaBarUI.gameObject.SetActive(true);
        }
    }
    public void miniGame()
    {        
        if (staminaBarUI.currentStamina <= 15 && peopleControllers.topPeople == 1 && LastStand == true)
        {
            miniG1.SetActive(false);
            miniG2.SetActive(true);
            LastStand = false;
            if (ModeLastStand == true)
            {
                LastStandMode = true;
            }
            spamSkillCheck.staminaGainAmount += 2;
        }
        if (staminaBarUI.currentStamina >= 85 || LastStandMode == true)
        {
            miniG1.SetActive(false);
            miniG2.SetActive(true);
            if (staminaBarUI.currentStamina >= 30 && LastStandMode == true)
            {
                LastStandMode = false;
                spamSkillCheck.staminaGainAmount -= 2;
            }
        }
        else
        {
            miniG1.SetActive(true);
            miniG2.SetActive(false);
        }


        if (peopleControllers.topPeople <= 0 || peopleControllers.bottomPeople <= 0)
        {
            miniG2.SetActive(false);
            miniG1.SetActive(false);
            staminaBarUI.gameObject.SetActive(false);
        }
    }
}
