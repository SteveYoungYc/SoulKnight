using System;
using UnityEngine;
using UnityEngine.UI;

public class SlideBar : MonoBehaviour
{
    public Transform target;
    public RectTransform rectTransform;
    public Image image;
    public Vector3 offset;
    public float width;
    public float ratio;

    void Start()
    {
        rectTransform.position = target.transform.position + offset;
        ratio = 1.0f;
    }

    void Update()
    {
        rectTransform.position = target.transform.position + offset;
        rectTransform.sizeDelta = new Vector2(width * ratio, rectTransform.sizeDelta.y);
    }
}
