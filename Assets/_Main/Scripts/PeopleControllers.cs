using TMPro;
using UnityEngine;

public class PeopleControllers : MonoBehaviour
{
    public int topPeople;
    public int bottomPeople;
    public int maxPeople;

    public TMP_Text remainingText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        remainingText.text = $"Remaining:\n {bottomPeople}";
    }
}
