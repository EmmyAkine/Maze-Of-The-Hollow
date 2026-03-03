using System;
using UnityEngine;

public class QuadrantSpawner : MonoBehaviour
{

    [SerializeField] protected GameObject prefab;


    [SerializeField] protected int itemsPerQuadrant = 0;
    [SerializeField] protected Transform itemParentHolder;
    private MazeGenerator mazeGenerator;

    [SerializeField] protected String spawnedItemName;

    protected Grid grid;

    private void Awake()
    {
        // Pre-instantiate all objects (fixed value X) in batch for memory efficiency
        mazeGenerator = FindFirstObjectByType<MazeGenerator>();
        if (mazeGenerator == null || prefab == null)
        {
            Debug.LogError($"Missing MazeGenerator {mazeGenerator} or Prefab {prefab}!");
            return;
        }

        
        SpawnItem();


    }

    protected virtual void Start()
    {
        grid = mazeGenerator.GetGrid();
        // Position and activate after maze is fully generated (Start runs after all Awakes)
        foreach (Transform item in itemParentHolder)
        {
            if (!item.gameObject.activeSelf) // Only process deactivated/pre-spawned
            {
                Grid.Quadrant quad = GetAssignedQuadrant(item);
                Grid.QuadrantBounds bounds = grid.GetQuadrantBounds(quad);
                Vector3 spawnPos = GetValidSpawnPosition(bounds);

                item.position = spawnPos;
                item.gameObject.SetActive(true);
            }
        }
    }

    private Vector3 GetValidSpawnPosition(Grid.QuadrantBounds bounds) //Remove Grid.Quadrant quad later, currently using for debugging
    {
        Vector3 spawnPos = Vector3.zero;
        bool validSpawn = false;

        const int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            int randX = UnityEngine.Random.Range(bounds.minX, bounds.maxX + 1);
            int randY = UnityEngine.Random.Range(bounds.minY, bounds.maxY + 1);
            spawnPos = grid.GetWorldPosition(randX, randY) + new Vector3(grid.CellSize / 2f, grid.CellSize / 2f, 0);

            Cell cell = grid.GetCell(randX, randY);

            if (cell != null && (!cell.north || !cell.east || !cell.south || !cell.west))  // At least one open wall
            {
                validSpawn = true;
                break;
            }
            else
            {
                validSpawn = false;
                continue;
            }

        }
        
        if (!validSpawn)
        {
            // Fallback center
            float centerX = (bounds.minX + bounds.maxX) / 2f * grid.CellSize + grid.CellSize / 2f;
            float centerY = (bounds.minY + bounds.maxY) / 2f * grid.CellSize + grid.CellSize / 2f;
            spawnPos = new Vector3(centerX, centerY, 0);
            return spawnPos;
        }
        return spawnPos;
    }

    protected virtual Grid.Quadrant GetAssignedQuadrant(Transform item)
    {
        return default;
    }

    protected void SpawnItem()
    {
        foreach (Grid.Quadrant quad in System.Enum.GetValues(typeof(Grid.Quadrant)))
        {
            for (int i = 0; i < itemsPerQuadrant; i++)
            {
                // Instantiate directly (pre-alloc in Awake avoids playtime hit)
                GameObject spawnedItem = Instantiate(prefab, itemParentHolder, true);
                spawnedItem.name = spawnedItemName;
                spawnedItem.SetActive(false);  // Deactivate initially for spawn control
                AssignedQuadrantSetter(spawnedItem, quad);

            }
        }
    }


    

    
    protected virtual void AssignedQuadrantSetter(GameObject spawnedItem, Grid.Quadrant quad)
    {

    }


}
