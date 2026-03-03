using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFindingMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float pathRecalcInterval = 0.5f;  // Optional: Recalc if moving and interval passed

    private MazeGenerator mazeGenerator;
    private PathFinding pathFinding;
    private List<Cell> currentPath;
    private int currentPathIndex = 0;
    private Vector3 currentTargetWorldPos;  // Last set target for recalc checks
    private float lastPathRecalcTime;

    void Start()
    {
        mazeGenerator = FindFirstObjectByType<MazeGenerator>();
        if (mazeGenerator != null)
        {
            pathFinding = new PathFinding(mazeGenerator.GetGrid());
        }
        else
        {
            Debug.LogError("MazeGenerator not found!");
        }
    }

    private void Update()
    {
        // Handle movement if path exists
        if (currentPath != null && currentPathIndex < currentPath.Count)
        {
            Vector3 nextCellCenter = GetCellCenter(currentPath[currentPathIndex]);
            transform.position = Vector3.MoveTowards(transform.position, nextCellCenter, moveSpeed * Time.deltaTime);

            // Advance index if reached
            if (Vector3.Distance(transform.position, nextCellCenter) < 0.1f)
            {
                currentPathIndex++;
            }
        }

        // Optional auto-recalc if path ended or interval passed (e.g., for dynamic targets like player)
        if (Time.time - lastPathRecalcTime >= pathRecalcInterval &&
            (currentPath == null || currentPathIndex >= currentPath.Count))
        {
            if (currentTargetWorldPos != Vector3.zero)  // Only if a target was set
            {
                CalculatePathToTarget(currentTargetWorldPos);
            }
        }
    }

    /// <summary>
    /// Sets a new target world position, calculates the path, and starts moving.
    /// Call this from EnemyAI when behavior changes or periodically.
    /// </summary>
    
    public void MoveTo(Vector3 targetPosition)
    {
        SetTarget(targetPosition);
    }
    private void SetTarget(Vector3 targetWorldPos)
    {
        currentTargetWorldPos = targetWorldPos;
        CalculatePathToTarget(targetWorldPos);
    }

    private void CalculatePathToTarget(Vector3 targetWorldPos)
    {
        Cell startCell = WorldToCell(transform.position);
        Cell goalCell = WorldToCell(targetWorldPos);

        if (startCell == null || goalCell == null)
        {
            return;
        }

        currentPath = pathFinding.FindPath(startCell, goalCell);
        if (currentPath != null)
        {
            currentPathIndex = currentPath.Count > 1 ? 1 : 0;  // Skip start if possible
            lastPathRecalcTime = Time.time;
        }
        else
        {
            currentPath = null;
        }
    }

    // Helper: World pos to Cell
    private Cell WorldToCell(Vector3 worldPos)
    {
        if (mazeGenerator == null) return null; 
        Grid currentGrid = mazeGenerator.GetGrid();
        if (currentGrid == null) return null;
        float cellSize = currentGrid.CellSize;
        int x = Mathf.FloorToInt(worldPos.x / cellSize);
        int y = Mathf.FloorToInt(worldPos.y / cellSize);
        return currentGrid.GetCell(x, y);
    }

    // Helper: Cell to center world pos
    private Vector3 GetCellCenter(Cell cell)
    {
        if (mazeGenerator == null || cell == null) return transform.position;
        Grid currentGrid = mazeGenerator.GetGrid();
        if (currentGrid == null) return transform.position;
        Vector3 pos = currentGrid.GetWorldPosition(cell.x, cell.y);
        float halfSize = currentGrid.CellSize / 2f;
        return pos + new Vector3(halfSize, halfSize, 0);
    }
}
