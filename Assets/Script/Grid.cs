using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    LayerMask whatIsObsticle;

    Vector2 gridSize;
    int width, height;
    int cellSize;
    public Cell[,] grid;
    // Start is called before the first frame update
    void Awake()
    {
        CreateGrid(10, 10, 2);
        NeighbourCells(grid[5, 5]);
    }

    public void CreateGrid(int _width, int _height, int _cellSize)
    {
        width = _width;
        height = _height;
        cellSize = _cellSize;

        gridSize = new Vector2(width * cellSize, height * cellSize);
        Vector2 leftBottomPosition = new Vector2(transform.position.x, transform.position.y) - (Vector2.right * gridSize.x / 2) - (Vector2.up * gridSize.y / 2);
        //GameObject Object = new GameObject();
        //GameObject NewObject1 = Instantiate(Object, new Vector3(leftBottomPosition.x, leftBottomPosition.y, 0), Quaternion.identity);
        //NewObject1.name = "leftBottomPosition";
        
        grid = new Cell[this.width, this.height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPosition = leftBottomPosition + Vector2.up * x * cellSize + Vector2.right * y * cellSize;
                RaycastHit2D checkForObsticles = Physics2D.BoxCast(worldPosition, new Vector2(cellSize, cellSize), 0f, Vector2.zero, 1f, whatIsObsticle);
                bool isObsticleThere = checkForObsticles;
                grid[x, y] = new Cell(!isObsticleThere, worldPosition, x, y);
                //GameObject NewObject = Instantiate(Object, new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
                //NewObject.name = "[" + x + " " + y + "]";
                if (isObsticleThere)
                {
                    DrawRectangle(worldPosition, cellSize, Color.black);
                }
                else
                {
                    DrawRectangle(worldPosition, cellSize, Color.red);

                }
            }
        }
    }
    
    private void DrawRectangle(Vector2 centerOfRectangle, int size, Color color)
    {
        //Debug.LogFormat("Drawing rectangle on Position {0}\nWith Size: {1}", centerOfRectangle, size);
        Vector2 pointA = centerOfRectangle - Vector2.up * size / 2 - Vector2.right * size / 2;
        Vector2 pointB = centerOfRectangle - Vector2.down * size / 2 - Vector2.right * size / 2;

        Vector2 pointC = centerOfRectangle - Vector2.up * size / 2 - Vector2.left * size / 2;
        Vector2 pointD = centerOfRectangle - Vector2.down * size / 2 - Vector2.left * size / 2;

        Debug.DrawLine(pointA, pointB, color, 100f);
        Debug.DrawLine(pointB, pointD, color, 100f);
        Debug.DrawLine(pointD, pointC, color, 100f);
        Debug.DrawLine(pointC, pointA, color, 100f);
        
    }

    public int DistanceBtwCells(Cell cellA, Cell cellB)
    {
        if (cellA == cellB)
            return -1;
        int xDistance = Mathf.Abs(cellB.xPos - cellA.xPos);
        int yDistance = Mathf.Abs(cellB.yPos - cellA.yPos);
        
        if(xDistance > yDistance)
        {
            return 14 * yDistance + (xDistance - yDistance) * 10;
        }

        return 14 * xDistance + (yDistance - xDistance) * 10;
    }

    public Cell CellFromWorldPos(Vector3 position)
    {
        //Debug.LogFormat("Grid size is\n X: {0} \nY: {1}", gridSize.x, gridSize.y);
        float percentX = (position.x + gridSize.x / 2) / gridSize.x;
        float percentY = (position.y  + gridSize.y / 2) / gridSize.y;
        //Debug.LogFormat("percentX is {0} \npercentY: {1}", percentX, percentY);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        
        int xVal = Mathf.RoundToInt(width * percentX);
        int yVal = Mathf.RoundToInt(height * percentY);
        
        //Debug.LogFormat("X value is {0}\nY value is {1}", xVal, yVal);

        Cell theCell = grid[yVal, xVal];
        //DrawRectangle(theCell.cellGlobalPosition, cellSize, col);
        return theCell;               
    }

    public List <Cell> NeighbourCells(Cell cell)
    {
        List<Cell> neighbours = new List<Cell>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = cell.xPos + x;
                int checkY = cell.yPos + y;
                //Debug.LogFormat("Check X: {0}/nCheck Y: {1}/nWidth: {2}/nHeight: {3}", checkX, checkY, width, height);
                if (checkX < width && checkX >= 0 && checkY < height && checkY >= 0)
                {
                    neighbours.Add(grid[checkX, checkY]);
                    //DrawRectangle(grid[checkX, checkY].cellGlobalPosition, cellSize, Color.blue);
                }
            }
        }
        return neighbours;
    }
}

public class Cell
{
    public bool isWalkable;
    public Vector2 cellGlobalPosition;
    public Cell pastCell;
    public int xPos, yPos;
    public int gCost, hCost;
    public Cell(bool isWalkable, Vector2 cellGlobalPosition, int xPos, int yPos)
    {
        this.isWalkable = isWalkable;
        this.cellGlobalPosition = cellGlobalPosition;
        this.xPos = xPos;
        this.yPos = yPos;
    }
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}