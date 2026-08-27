using UnityEngine;

public class IsWinner : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public StaminaBarUI staminaBarUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckWinLose();
    }
    public void CheckWinLose()
    {
        if (staminaBarUI.staminaDrainRate == 0)
        {
            win.SetActive(true);
            lose.SetActive(false);
        }
        else if (staminaBarUI.staminaDrainRate >= 0.02f)
        {
            lose.SetActive(true);
            win.SetActive(false);
        }
    }
}
