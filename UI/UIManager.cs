using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas screenCanvas;
    public Canvas worldCanvas;

    private void Awake()
    {
        GameObject screenCanvasObj = new GameObject("ScreenCanvas", typeof(Canvas));
        screenCanvas = screenCanvasObj.GetComponent<Canvas>();
        screenCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        screenCanvas.worldCamera = GameManager.cameraMain;
        GameObject worldCanvasObj = new GameObject("WorldCanvas", typeof(Canvas));
        worldCanvas = worldCanvasObj.GetComponent<Canvas>();
        worldCanvas.renderMode = RenderMode.WorldSpace;
        worldCanvas.worldCamera = GameManager.cameraMain;
    }
}
