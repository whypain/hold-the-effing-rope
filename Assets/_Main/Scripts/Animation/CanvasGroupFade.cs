using System.Threading;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class CanvasGroupFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField, Range(0, 1)] private float startValue;
    [SerializeField, Range(0, 1)] private float endValue;
    [SerializeField, Range(0, 1)] private Ease ease = Ease.InOutSine;
    [SerializeField] private float  fadeDuration = 0.5f;
    [SerializeField] private bool fadeOnStart = true;

    private CancellationTokenSource fadeCts;

    void Start()
    {
        fadeCts = new CancellationTokenSource();
        if (fadeOnStart)
        {
            Fade();
        }
    }

    public void Fade()
    {
        StopFading();
        Sequence.Create()
            .Chain(Tween.Alpha(canvasGroup, startValue, endValue, fadeDuration, ease))
            .SetCancellationToken(fadeCts.Token);
    }

    public async Task FadeIn()
    {
        StopFading();
        await Sequence.Create()
            .Chain(Tween.Alpha(canvasGroup, 0, 1, fadeDuration, ease))
            .SetCancellationToken(fadeCts.Token);
    }

    public async Task FadeOut()
    {
        StopFading();
        await Sequence.Create()
            .Chain(Tween.Alpha(canvasGroup, 1, 0, fadeDuration, ease))
            .SetCancellationToken(fadeCts.Token);
    }

    public void StopFading()
    {
        fadeCts?.Cancel();
        fadeCts = new CancellationTokenSource();
    }

    private void OnValidate()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}
