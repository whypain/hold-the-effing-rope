using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeShake : MonoBehaviour
{
    [SerializeField] private Vector3 strength = 0.1f * Vector3.one;
    [SerializeField] private float frequency = 10f;

    [SerializeField] private bool _shakeOnStart = true;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_shakeOnStart)
        {
            Shake();
        }
    }

    public void Shake()
    {
        ShakeSettings settings = new(strength, 1f, frequency, false);
        Vector3 startPosition = lineRenderer.GetPosition(0);

        Sequence.Create()
            .Chain(Tween.ShakeCustom(lineRenderer, Vector3.zero, settings, (lr, v) =>
            {
                lr.SetPosition(0, startPosition + v);
            }))
            .SetRemainingCycles(-1);
    }
}