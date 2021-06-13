using System.Collections.Generic;
using UnityEngine;

public class UIGrid : MonoBehaviour
{

    public int noOfCells = 42;
    public int aOI = 1;
    public GameObject cellPrefab;
    public Transform cellParent;

    private static UIGrid instance;
    private const int noOfColumns = 5;
    private CellData[] m_cellData;
    private UICell[] m_cells;
    

    public static UIGrid GetInstance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<UIGrid>();
            return instance;
        }
    }

    void Start()
    {
       CreateGrid(noOfCells);
    }

    public void CreateGrid(int cellCount)
    {
        m_cellData = new CellData[noOfCells];
        m_cells = new UICell[noOfCells];
        for (int i = 0; i < noOfCells; i++)
        {
            CellData cData = new CellData(i, GetRandomColorData());
            m_cellData[i] = cData;

            GameObject go = Instantiate(cellPrefab);
            m_cells[i] = go.GetComponent<UICell>();
            m_cells[i].SetData(i);

            go.transform.SetParent(cellParent, false);
        }
    }

    public void CellClicked(int cellId)
    {
        m_cells[cellId].SetSelected(m_cellData[cellId].colData);
    }
    

    public ColorData GetRandomColorData()
    {
        return new ColorData(UnityEngine.Random.Range(0, 256),
                UnityEngine.Random.Range(0, 256),
                UnityEngine.Random.Range(0, 256));
    }
}

public struct ColorData
{
    public int r;
    public int g;
    public int b;

    public ColorData(int _r, int _g, int _b)
    {
        r = _r;
        g = _g;
        b = _b;
    }

    //public override string ToString()
    //{
    //    return string.Format("r:{0},g:{1},b:{2}", r, g, b);
    //}
}

public struct CellData
{
    public int index;
    public ColorData colData;

    public CellData(int _index, ColorData _colData)
    {
        index = _index;
        colData = _colData;
    }
}

public struct int2
{
    public int x;
    public int y;

    public int2(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
