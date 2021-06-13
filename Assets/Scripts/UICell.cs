using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UICell : MonoBehaviour, ICell
{

    public Image image;
    public Text text;

    private int m_cellId;

    public void SetData(int cellId)
    {
        m_cellId = cellId;
        SetToNormal();
    }

    public void SetSelected(ColorData colData)
    {
        image.color = new Color(colData.r / 255f, colData.g / 255f, colData.b / 255f);
        text.text = (m_cellId + 1).ToString();
    }

    public void SetToNormal()
    {
        image.color = Color.white;
        text.text = string.Empty;
    }

    public void OnClicked()
    {
        UIGrid.GetInstance.CellClicked(m_cellId);
    }
}