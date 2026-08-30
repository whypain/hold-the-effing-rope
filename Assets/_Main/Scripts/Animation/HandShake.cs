using PrimeTween;
using UnityEngine;

public class HandShake : MonoBehaviour
{
    [SerializeField] private Vector3 strength = 0.1f * Vector3.one;
    [SerializeField] private float frequency = 10f;

    [SerializeField] private bool _shakeOnStart = true;

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
        Sequence.Create()
            .Chain(Tween.ShakeLocalPosition(transform, strength, 1f, frequency, false))
            .SetRemainingCycles(-1);
    }
}
