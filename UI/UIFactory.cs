using System;
using UnityEngine;
using UnityEngine.UI;

public enum UIType
{
    PlayerHealthBar,
    EnemyHealthBar
}

public class UIFactory
{
    private readonly Canvas worldCanvas;
    private static UIFactory _instance;
    
    private UIFactory()
    {
        worldCanvas = GameManager.uiManager.worldCanvas;
    }
    
    public static UIFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIFactory();
            }
            return _instance;
        }
    }

    public SlideBar CreateSlideBar(UIType type, Transform target, Vector3 offset)
    {
        GameObject gameObject = new GameObject(type.ToString());
        SlideBar slideBar = gameObject.AddComponent<SlideBar>();
        gameObject.transform.SetParent(GameManager.uiManager.worldCanvas.transform);
        slideBar.image = gameObject.AddComponent<Image>();
        slideBar.rectTransform = gameObject.GetComponent<RectTransform>();
        slideBar.target = target;
        slideBar.offset = offset;
        switch (type)
        {
            case UIType.PlayerHealthBar:
                break;
            case UIType.EnemyHealthBar:
                slideBar.image.color = Color.red;
                slideBar.rectTransform.pivot = new Vector2(0, 0.5f);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        return slideBar;
    }
}
