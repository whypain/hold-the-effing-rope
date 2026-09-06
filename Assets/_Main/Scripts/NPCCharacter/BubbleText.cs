using PrimeTween;
using TMPro;
using UnityEngine;

public class BubbleText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Configs")]
    [SerializeField] private float randomOffset;
    [SerializeField] private float impactDuration;
    [SerializeField] private float animDuration;
    [SerializeField] private float punchStrength;
    [SerializeField] private float shakeStrength;
    [SerializeField] private float rotationShakeStrength;

    private Vector3 startingPos;

    private void Awake()
    {
        startingPos = transform.localPosition;
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    [ContextMenu("Show")]
    public void Show()
    {
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);

        if (startingPos == Vector3.zero) { startingPos = transform.localPosition; }
        float offset = Random.Range(-randomOffset, randomOffset);
        transform.localPosition = startingPos + new Vector3(canvas.scaleFactor * offset, canvas.scaleFactor * offset, 0f);

        Sequence.Create()
            .Chain(Tween.PunchScale(transform, Vector3.one * punchStrength, impactDuration))
            .Group(Tween.Alpha(canvasGroup, 1f, impactDuration))
            .Group(Tween.ShakeLocalPosition(transform, Vector3.one * shakeStrength, animDuration))
            .Group(Tween.ShakeLocalRotation(transform, Vector3.one * rotationShakeStrength, animDuration))
            .Chain(Tween.Alpha(canvasGroup, 0f, impactDuration))
            .OnComplete(() =>  {
                gameObject.SetActive(false);
                transform.localPosition = startingPos;
            });
    }
}
