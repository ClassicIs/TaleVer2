using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Grid : MonoBehaviour
{
    [SerializeField]
    LayerMask whatIsObsticle;
    Vector2 gridSize;
    int width, height;
    int cellSize;
    public Cell[,] grid;

    /*[SerializeField]
    GameObject theObject;*/

    public void CreateGrid(int _size, int _cellSize, Vector3 position)
    {
        width = _size;
        height = _size;
        cellSize = _cellSize;
        
        gridSize = new Vector2(width * cellSize, height * cellSize);

        transform.position = position;
        Vector2 leftBottomPosition = new Vector2(transform.position.x, transform.position.y) - (Vector2.right * gridSize.x / 2) - (Vector2.up * gridSize.y / 2);
        
        grid = new Cell[this.width, this.height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPosition = leftBottomPosition + Vector2.up * x * cellSize + Vector2.right * y * cellSize;
                RaycastHit2D checkForObsticles = Physics2D.BoxCast(worldPosition, new Vector2(cellSize, cellSize), 0f, Vector2.zero, 1f, whatIsObsticle);
                bool isObsticleThere = checkForObsticles;
                grid[x, y] = new Cell(!isObsticleThere, worldPosition, x, y);
                
                if (isObsticleThere)
                {
                    /*TextMeshPro theText = Instantiate(text, worldPosition, Quaternion.identity);
                    theText.text = x.ToString() + " " + y.ToString();
                    theText.color = Color.black;
                    GameObject obj = Instantiate(theObject, worldPosition, Quaternion.identity);
                    obj.name = x.ToString() + " " + y.ToString();*/
                    DrawRectangle(worldPosition, cellSize, Color.black, 4f);
                }
                else
                {
                    /*TextMeshPro theText = Instantiate(text, worldPosition, Quaternion.identity);
                    theText.text = x.ToString() + " " + y.ToString();
                    theText.color = Color.red;*/
                    /*GameObject obj = Instantiate(theObject, worldPosition, Quaternion.identity);
                    obj.name = x.ToString() + " " + y.ToString();*/
                    DrawRectangle(worldPosition, cellSize, Color.red, 4f);

                }
            }
        }
    }
    
    public void DrawRectangle(Vector2 centerOfRectangle, int size, Color color, float time = 100)
    {
        Vector2 pointA = centerOfRectangle - Vector2.up * size / 2 - Vector2.right * size / 2;
        Vector2 pointB = centerOfRectangle - Vector2.down * size / 2 - Vector2.right * size / 2;

        Vector2 pointC = centerOfRectangle - Vector2.up * size / 2 - Vector2.left * size / 2;
        Vector2 pointD = centerOfRectangle - Vector2.down * size / 2 - Vector2.left * size / 2;
        
        Debug.DrawLine(pointA, pointB, color, time);
        Debug.DrawLine(pointB, pointD, color, time);
        Debug.DrawLine(pointD, pointC, color, time);
        Debug.DrawLine(pointC, pointA, color, time);
        
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

    public Cell CellFromWorldPos(Vector3 worldPosition)
    {
        Vector2 worldPosition2 = new Vector2(worldPosition.x, worldPosition.y);
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if(Vector2.Distance(grid[i, j].cellGlobalPosition, worldPosition2) < cellSize/1.5f)
                {
                    return grid[i, j];
                }
            }
        }
        return null;
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