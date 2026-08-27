using UnityEngine;

public class IsWinner : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public PeopleControllers peopleControllers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        win.SetActive(false);
        lose.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckWinLose();
    }
    public void CheckWinLose()
    {
        if (peopleControllers.topPeople == peopleControllers.maxPeople)
        {
            win.SetActive(true);
            lose.SetActive(false);
        }
        else if (peopleControllers.bottomPeople == peopleControllers.maxPeople)
        {
            lose.SetActive(true);
            win.SetActive(false);
        }
    }
}
