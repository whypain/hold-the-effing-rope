using UnityEngine;

public class BubbleTextManager : MonoBehaviour
{
    [SerializeField] private BubbleText[] bubbles;

    private void OnEnable()
    {
        GameStateManager.Instance.OnPeopleCountChanged += ShowBubble;
    }

    private void OnDisable()
    {
        GameStateManager.Instance.OnPeopleCountChanged -= ShowBubble;
    }

    private void ShowBubble()
    {
        int bottomPeople = GameStateManager.Instance.GS.bottomPeople;
        if (bottomPeople <= 0) return;

        BubbleText randomBubble = bubbles[Random.Range(0, bubbles.Length)];
        randomBubble.SetText(bottomPeople.ToString());
        randomBubble.Show();
    }
}
