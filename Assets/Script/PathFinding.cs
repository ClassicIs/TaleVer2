using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField]
    Grid grid;
    [SerializeField]
    Transform startPosition;
    [SerializeField]
    Transform endPosition;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
        //FindPath(startPosition.position, endPosition.position);
        //StartCoroutine(FindCoroutine());
    }
    
    IEnumerator FindCoroutine()
    {
        while (true)
        {
            Vector3[] points = FindPath(startPosition.position, endPosition.position);
            if (points != null)
            {
                /*for (int i = 0; i < points.Count; i++)
                {
                    Debug.LogFormat("Point {0} is {1}", i, points[i]);
                }*/
                DrawLineByPoint(points);
            }
            yield return null;
        }
    }

    private void Update()
    {
        /*
        Vector3[] points = FindPath(startPosition.position, endPosition.position);
        if (points != null)
        {
            DrawLineByPoint(points);
        }*/
    }

    private void DrawLineByPoint(Vector3[] points, float wait = 1f)
    {
        for(int i = 0; i < points.Length - 1; i++)
        {
            Debug.DrawLine(points[i], points[i + 1], Color.green, wait);
        }
    }
    /*
    public List <Vector3> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        List<Vector3> path = new List<Vector3>();

        List<Cell> openList = new List<Cell>();

        HashSet<Cell> closeList = new HashSet<Cell>();

        Cell startCell = grid.CellFromWorldPos(startPosition, Color.green);

        Cell endCell = grid.CellFromWorldPos(endPosition, Color.cyan);
        openList.Add(startCell);

        while(openList.Count > 0)
        {
            Cell cell = openList[0];

            for(int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < cell.fCost || openList[i].fCost == cell.fCost)
                {
                    if(openList[i].hCost < cell.hCost)
                    {
                        cell = openList[i];
                    }
                }
            }

            openList.Remove(cell);
            closeList.Add(cell);

            if(cell == endCell)
            {
                Debug.Log("Path was found!");
                return RetracePath(endCell);
            }
            foreach(Cell neighbour in grid.NeighbourCells(cell))
            {
                if(!neighbour.isWalkable || closeList.Contains(cell))
                {
                    continue;
                }

                int newCostToNeighbour = cell.gCost + grid.DistanceBtwCells(cell, neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = grid.DistanceBtwCells(neighbour, endCell);
                    neighbour.pastCell = cell;
                    if(!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }

        }
        //grid.CellFromWorldPos(findThisPosition.position); 
        Debug.Log("There's no path!");
        return null;
    }*/

    public Vector3[] FindPath(Vector3 startPos, Vector3 targetPos)
    {
        grid.CreateGrid(20, 2, startPos);
        
        Cell startCell = grid.CellFromWorldPos(startPos);
        Cell targetCell = grid.CellFromWorldPos(targetPos);

        //grid.DrawRectangle(startCell.cellGlobalPosition, 2, Color.green, 10);
        //grid.DrawRectangle(targetCell.cellGlobalPosition, 2, Color.cyan, 10);

        List<Cell> openSet = new List<Cell>();
        HashSet<Cell> closedSet = new HashSet<Cell>();
        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            Cell node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetCell)
            {
                Debug.Log("Path was found!");

                return RetracePath(startCell, targetCell);
                
            }

            foreach (Cell neighbour in grid.NeighbourCells(node))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + grid.DistanceBtwCells(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = grid.DistanceBtwCells(neighbour, targetCell);
                    neighbour.pastCell = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        Debug.Log("Path was NOT found!");
        return null;

    }

    public Vector3[] RetracePath(Cell startCell, Cell endCell)
    {
        List<Vector3> path = new List<Vector3>();
        Cell curCell = endCell;
        while(curCell != startCell)
        {
            path.Add(curCell.cellGlobalPosition);
            curCell = curCell.pastCell;
        }
        path.Reverse();
        Vector3[] newPath = path.ToArray();
        //DrawLineByPoint(newPath, 100f);
        return newPath;
    }


}
