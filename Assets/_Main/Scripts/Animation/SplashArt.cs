using System.Threading;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class SplashArt : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("BounceIn Config")]
    [SerializeField] private Vector3 bounceDirection = Vector3.one;
    [SerializeField] private float bounceInDuration = 0.5f;
    [SerializeField] private float bounceInScaleFactor = 1.2f;

    [Header("BounceStay Config")]
    [SerializeField] private Vector3 bounceStayDirection = Vector3.one;
    [SerializeField] private float bounceStayDuration = 0.5f;
    [SerializeField] private float bounceStayScaleFactor = 1.2f;
    [SerializeField] private float bounceStayFrequency = 5f;

    [Header("ShakeStay Config")]
    [SerializeField] private Vector3 shakeStayDirection = Vector3.one;
    [SerializeField] private float shakeStayDuration = 0.5f;
    [SerializeField] private float shakeStayFrequency = 5f;
    [SerializeField] private float shakeStayScaleFactor = 1.2f;



    private CancellationTokenSource stayCts;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    [ContextMenu("Bounce In")]
    public async Task BounceIn()
    {
        await Sequence.Create()
            .Chain(Tween.PunchScale(canvasGroup.transform, bounceDirection * bounceInScaleFactor, bounceInDuration));
    }

    [ContextMenu("Bounce Stay")]
    public void BounceStay()
    {
        stayCts ??= new CancellationTokenSource();
        Sequence.Create()
            .Chain(Tween.ShakeScale(canvasGroup.transform, bounceStayDirection * bounceStayScaleFactor, bounceStayDuration, bounceStayFrequency, false))
            .SetCancellationToken(stayCts.Token)
            .SetRemainingCycles(-1);
    }

    [ContextMenu("Shake Stay")]
    public void ShakeStay()
    {
        stayCts ??= new CancellationTokenSource();
        Sequence.Create()
            .Chain(Tween.ShakeLocalPosition(canvasGroup.transform, shakeStayDirection * shakeStayScaleFactor, shakeStayDuration, shakeStayFrequency, false))
            .SetCancellationToken(stayCts.Token)
            .SetRemainingCycles(-1);
    }

    [ContextMenu("Stop Stay Anim")]
    public void StopStayAnim()
    {
        stayCts?.Cancel();
        stayCts = new CancellationTokenSource();

        canvasGroup.transform.localScale = Vector3.one;
    }

    [ContextMenu("Record Original Transform")]
    private void RecordOriginalTransform()
    {
        originalScale = canvasGroup.transform.localScale;
        originalPosition = canvasGroup.transform.localPosition;
    }

    [ContextMenu("Reset Transform")]
    private void Reset()
    {
        canvasGroup.transform.localScale = originalScale;
        canvasGroup.transform.localPosition = originalPosition;
    }
}
