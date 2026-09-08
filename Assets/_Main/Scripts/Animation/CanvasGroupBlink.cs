using System.Threading;
using PrimeTween;
using UnityEngine;

public class CanvasGroupBlink : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float  blinkMin = 0.1f;
    [SerializeField] private float  blinkMax = 1f;
    [SerializeField] private float  blinkDuration = 0.5f;
    [SerializeField] private bool blinkOnStart = true;

    private CancellationTokenSource blinkCts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blinkCts = new CancellationTokenSource();
        if (blinkOnStart)
        {
            Blink();
        }
    }

    public void Blink()
    {
        StopBlinking();
        Sequence.Create()
            .Chain(Tween.Alpha(canvasGroup, startValue: blinkMax, endValue: blinkMin, blinkDuration))
            .Chain(Tween.Alpha(canvasGroup, startValue: blinkMin, endValue: blinkMax, blinkDuration))
            .SetCancellationToken(blinkCts.Token)
            .SetRemainingCycles(-1);
    }

    public void StopBlinking()
    {
        blinkCts?.Cancel();
        blinkCts = new CancellationTokenSource();

        canvasGroup.alpha = 0f;
    }

    private void OnValidate()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}
