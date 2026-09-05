using TMPro;
using UnityEngine;

public class Remaining : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingText;

    void Update()
    {
        remainingText?.SetText($"Remaining: \n{GlobalState.Instance.bottomPeople}");
    }
}
