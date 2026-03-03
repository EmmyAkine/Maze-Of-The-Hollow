using UnityEngine;

public class LivesSpawner : QuadrantSpawner
{
    protected override void AssignedQuadrantSetter(GameObject spawnedItem, Grid.Quadrant quad)
    {
        Lives live = prefab.GetComponent<Lives>();
        if (live != null)
        {
            live.assignedQuadrant = quad;
        }
    }

    protected override Grid.Quadrant GetAssignedQuadrant(Transform item)
    {
        return item.GetComponent<Lives>().assignedQuadrant;
    }

    protected override void Start()
    {
        base.Start();
    }

    public int GetTotalLives()
    {
        return itemParentHolder.childCount;
    }
}
