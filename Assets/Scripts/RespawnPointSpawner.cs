using UnityEngine;

public class RespawnPointSpawner : QuadrantSpawner
{
    protected override void AssignedQuadrantSetter(GameObject spawnedItem, Grid.Quadrant quad)
    {
        RespawnPoint respawnPoint = prefab.GetComponent<RespawnPoint>();
        if (respawnPoint != null)
        {
            respawnPoint.assignedQuadrant = quad;
        }
        else
        {
            Debug.LogWarning("RespawnPoint missing on respawnpoint prefab!");
        }
    }

    protected override Grid.Quadrant GetAssignedQuadrant(Transform item)
    {
        return item.GetComponent<RespawnPoint>().assignedQuadrant;
    }

    protected override void Start()
    {
        base.Start();
    }


}
