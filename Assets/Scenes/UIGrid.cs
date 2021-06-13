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
    private HashSet<CellData> m_selectedCells;
    private Point2[] m_neighbourOffsets;



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
        m_neighbourOffsets = new Point2[] {
            new Point2(-1,-1),    // top left
            new Point2(0,-1),     // top center
            new Point2(1,-1),     // top right
            new Point2(-1,0),     // left
            new Point2(1,0),      // right
            new Point2(-1,1),     // bottom left
            new Point2(0,1),      // bottom
            new Point2(1,1)       // bottom right

        };
        CreateGrid(noOfCells);
    }

    public void CreateGrid(int cellCount)
    {
        m_cellData = new CellData[noOfCells];
        m_cells = new UICell[noOfCells];
        for (int i = 0; i < noOfCells; i++)
        {
            CellData cData = new CellData(i, new Point2(i % noOfColumns, i / noOfColumns), GetRandomColorData());
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

        ClearPrevCells();

        m_selectedCells = GetNeighbours(m_cellData[cellId].position, aOI);
        m_selectedCells.Add(m_cellData[cellId]);
        Debug.Log(m_selectedCells.Count);
        foreach (CellData c in m_selectedCells)
        {
            if (m_cells[c.index] != null)
            {
                m_cells[c.index].SetSelected(c.colData);
            }
        }
    }

    public HashSet<CellData> GetNeighbours(Point2 fromCellPos, int aOI)
    {
        HashSet<CellData> h_neighbours = new HashSet<CellData>();
        HashSet<CellData> h_closedCell = new HashSet<CellData>();
        if (aOI > 0)
        {
            foreach (Point2 neighbour in m_neighbourOffsets)
            {
                Point2 neighbourPos = fromCellPos + neighbour;

                if (IsValidPos(neighbourPos))
                {
                    //Debug.Log($"fromCell ({fromCellPos.x}, {fromCellPos.y})Neighbour pos: ({neighbourPos})");//.x},{neighbourPos.y})");
                    int index = GetIndexFromGridPos(neighbourPos);
                    if (index > -1 && index < noOfCells)
                    {
                        if (h_closedCell.Contains(m_cellData[index]))
                            continue;
                        h_closedCell.Add(m_cellData[index]);
                        h_neighbours.Add(m_cellData[index]);
                        if (aOI > 1)
                        {
                            h_neighbours.UnionWith(GetNeighbours(m_cellData[index].position, aOI - 1));
                        }
                    }
                    else
                    {
                        //invalid position
                    }
                }
            }
        }
        return h_neighbours;
    }

    private void ClearPrevCells()
    {
        if (m_selectedCells != null)
        {
            foreach (CellData c in m_selectedCells)
            {
                UICell uiCell = m_cells[c.index];
                if (uiCell != null)
                {
                    uiCell.SetToNormal();
                }
            }
            m_selectedCells.Clear();
            m_selectedCells = null;
        }
    }


    public ColorData GetRandomColorData()
    {
        return new ColorData(UnityEngine.Random.Range(0, 256),
                UnityEngine.Random.Range(0, 256),
                UnityEngine.Random.Range(0, 256));
    }

    private bool IsValidPos(Point2 pos)
    {
        if (pos.x > -1 && pos.x < noOfColumns &&
            pos.y > -1 && pos.y <= noOfCells / noOfColumns)
        {
            int index = pos.y * noOfColumns + pos.x;
            if (index > -1 && index < noOfCells)
            {
                return true;
            }
        }
        return false;
    }


    private int GetIndexFromGridPos(Point2 pos)
    {
        return pos.y * noOfColumns + pos.x;
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
    public Point2 position;

    public CellData(int _index, Point2 _pos, ColorData _colData)
    {
        index = _index;
        colData = _colData;
        position = _pos;
    }
}

public struct Point2
{
    public int x;
    public int y;

    public Point2(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static Point2 operator +(Point2 a, Point2 b)
    {
        return new Point2((a.x + b.x), (a.y + b.y));
    }
}
