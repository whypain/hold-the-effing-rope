using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zoomInSize = 3.5f;
    [SerializeField] private float zoomDuration = 0.5f;

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
}
