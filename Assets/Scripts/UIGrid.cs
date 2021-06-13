using System.Collections.Generic;
using UnityEngine;

public class UIGrid : MonoBehaviour, IRecyclableScrollRectDataSource
{
    public RecyclableScrollRect scroll;
    public int noOfCells = 42;
    [Range(1,5)]
    public int aOI = 1;

    private static UIGrid instance;
    private const int noOfColumns = 5;
    private CellData[] m_cellData;
    private HashSet<CellData> m_selectedCells;      // contails list of selected cells
    private Point2[] m_neighbourOffsets;            // Data structure to find neighbours


    public static UIGrid GetInstance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<UIGrid>();
            return instance;
        }
    }

    private void Start()
    {
        PopulateGrid(noOfCells, aOI);
    }

    public void PopulateGrid(int noOfCells, int aOI)
    {
        this.noOfCells = noOfCells;
        this.aOI = aOI;

        m_neighbourOffsets = new Point2[]
        {
            new Point2(-1,-1),    // top left
            new Point2(0,-1),     // top center
            new Point2(1,-1),     // top right
            new Point2(-1,0),     // left
            new Point2(1,0),      // right
            new Point2(-1,1),     // bottom left
            new Point2(0,1),      // bottom
            new Point2(1,1)       // bottom right
        };

        CreateCellData(noOfCells);

        scroll.DataSource = this;
        scroll.Initialize(this);
    }

    private void CreateCellData(int cellCount)
    {
        m_cellData = new CellData[noOfCells];

        for (int i = 0; i < noOfCells; i++)
        {
            Point2 gridPos = new Point2(i % noOfColumns, i / noOfColumns);
            CellData cData = new CellData(i, gridPos , GetRandomColorData());
            m_cellData[i] = cData;
        }
    }

    public void CellClicked(int cellId)
    {
        ClearPrevCells();
        
        m_selectedCells = GetNeighbours(m_cellData[cellId].position, aOI);
        m_selectedCells.Add(m_cellData[cellId]);
        foreach (CellData c in m_selectedCells)
        {
            UICell uiCell = scroll.GetCell(c.index) as UICell;
            if (uiCell != null)
            {
                uiCell.SetSelected(c.colData);
            }
        }
    }
    
    private HashSet<CellData> GetNeighbours(Point2 fromCellPos, int aOI)
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
                UICell uiCell = scroll.GetCell(c.index) as UICell;
                if (uiCell != null)
                {
                    uiCell.SetToNormal();
                }
            }
            m_selectedCells.Clear();
            m_selectedCells = null;
        }
       
    }


    private ColorData GetRandomColorData()
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

    public int GetItemCount()
    {
        return noOfCells;
    }

    public void SetCell(ICell cell, int index)
    {
        UICell c = cell as UICell;
        
        c.SetData(index);
        if (m_selectedCells != null && m_selectedCells.Contains(m_cellData[index]))
        {
            c.SetSelected(m_cellData[index].colData);
        }
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
