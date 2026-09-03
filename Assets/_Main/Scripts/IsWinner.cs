using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsWinner : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public GameObject replay;
    public GameObject startScreen;
    public GameObject remaining;
    public PeopleControllers peopleControllers;

    public bool GameOver;
    public bool gameStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScreen.SetActive(true);
        remaining.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
        replay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckWinLose();
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !gameStarted)
        {
            startScreen.SetActive(false); remaining.SetActive(true);
            gameStarted = true;
        }
    }

    public void CheckWinLose()
    {
        if (GameOver) return;

        if (peopleControllers.topPeople == peopleControllers.maxPeople)
        {
            win.SetActive(true);
            lose.SetActive(false);
            replay.SetActive(true);
            remaining.SetActive(false);
            AudioSystem.Instance?.Play(AudioType.Win);
            GameOver = true;
        }
        else if (peopleControllers.bottomPeople == peopleControllers.maxPeople)
        {
            lose.SetActive(true);
            win.SetActive(false);
            replay.SetActive(true);
            remaining.SetActive(false);
            AudioSystem.Instance?.Play(AudioType.Lose);
            GameOver = true;
        }
    }
}
