using UnityEngine;

public class GhostSpawner : QuadrantSpawner
{
    /*[SerializeField] private GameObject prefab;*/

    protected override void AssignedQuadrantSetter(GameObject spawnedItem, Grid.Quadrant quad)
    {
        EnemyAI ai = prefab.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.assignedQuadrant = quad;
        }
        else
        {
            Debug.LogWarning("EnemyAI missing on ghost prefab!");
        }

    }

    protected override Grid.Quadrant GetAssignedQuadrant(Transform item)
    {
        return item.GetComponent<EnemyAI>().assignedQuadrant;
    }

    protected override void Start()
    {
        base.Start();
    }
}