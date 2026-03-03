using UnityEngine;

public class KeysSpawner : QuadrantSpawner
{
    protected override void AssignedQuadrantSetter(GameObject spawnedItem, Grid.Quadrant quad)
    {
        Keys keys = prefab.GetComponent<Keys>();
        if (keys != null)
        {
            keys.assignedQuadrant = quad;
        }
        else
        {
            Debug.LogWarning("Key on key prefab is missing!!!");
        }
    }

    protected override Grid.Quadrant GetAssignedQuadrant(Transform item)
    {
        return item.GetComponent<Keys>().assignedQuadrant;
    }

    protected override void Start()
    {
        base.Start();
    }

    public int GetTotalKeys()
    {
        return itemParentHolder.childCount;
    }
}
