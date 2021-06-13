using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaRect : MonoBehaviour
{
    void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 anchorMin = Screen.safeArea.position;
        Vector2 anchorMax = anchorMin + Screen.safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
