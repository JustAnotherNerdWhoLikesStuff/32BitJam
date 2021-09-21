using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownsampleCamera : MonoBehaviour
{
    public Camera DownsamplingCamera;
    [Range(0.001F, 1.00F)]
    public float renderScale = 1.0F;
    public FilterMode filterMode = FilterMode.Point;

    private Rect originalScreen;
    private Rect downsampledScreen;

    void Start()
    {
        DownsamplingCamera = Camera.main;
    }

    void OnDestroy()
    {
        DownsamplingCamera.rect = originalScreen;
    }

    void OnPreRender()
    {
        originalScreen = DownsamplingCamera.rect;
        downsampledScreen.Set(originalScreen.x, originalScreen.y, originalScreen.width * renderScale, originalScreen.height * renderScale);
        DownsamplingCamera.rect = downsampledScreen;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        DownsamplingCamera.rect = originalScreen;
        src.filterMode = filterMode;
        Graphics.Blit(src, dest);
    }
}