using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zoomInSize = 3.5f;
    [SerializeField] private float zoomDuration = 0.5f;

    [Header("Camera Bobbing Settings")]
    [SerializeField] private float cameraBobbingMagnitude = 0.1f;
    [SerializeField] private float cameraBobbingFrequency = 1f;
    [SerializeField] private float cameraBobbingDuration = 5f;

    private float normalSize;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCamera ??= Camera.main;
        if (mainCamera != null)
        {
            normalSize = mainCamera.orthographicSize;
        }

        Sequence.Create()
            .Chain(Tween.ShakeCamera(mainCamera, cameraBobbingMagnitude, cameraBobbingDuration, cameraBobbingFrequency))
            .SetRemainingCycles(-1);
    }

    public async Task ZoomIn()
    {
        if (mainCamera == null) return;

        await Tween.CameraOrthographicSize(mainCamera, zoomInSize, zoomDuration);
    }

    public async Task ZoomOut()
    {
        if (mainCamera == null) return;

        await Tween.CameraOrthographicSize(mainCamera, normalSize, zoomDuration);
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        if (mainCamera == null) return;

        Tween.ShakeCamera(mainCamera, magnitude, duration);
    }
}
