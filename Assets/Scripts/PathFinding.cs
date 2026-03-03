using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private Grid grid;

    public PathFinding(Grid grid)
    {
        this.grid = grid;
    }

    private class Node
    {
        public Cell cell;
        public Node parent;

        public int gCost;
        public int hCost;

        public int fCost => gCost + hCost;

        public Node(Cell cell, Node parent, int gCost, int hCost)
        {
            this.cell = cell;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
        }
    }

    public List<Cell> FindPath(Cell start, Cell goal)
    {
        if (start == null || goal == null) return null;

        List<Node> openSet = new List<Node>();   
        HashSet<Cell> closedSet = new HashSet<Cell>();

        Node startNode = new Node(start, null, 0, GetHeuristic(start, goal));
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // Find node with lowest fCost (and lowest hCost on tie)
            Node current = GetLowestFCostNode(openSet);

            if (current.cell == goal)
            {
                // Reconstruct and return the path
                return ReconstructPath(current);
            }

            openSet.Remove(current);
            closedSet.Add(current.cell);

            // Get walkable neighbors
            List<Cell> neighbors = GetWalkableNeighbors(current.cell);
            foreach (Cell neighborCell in neighbors)
            {
                if (closedSet.Contains(neighborCell)) continue;

                int tentativeGCost = current.gCost + 1;  // Distance between nodes is always 1 in grid

                Node neighborNode = openSet.Find(n => n.cell == neighborCell);
                if (neighborNode == null)
                {
                    // New node
                    neighborNode = new Node(neighborCell, current, tentativeGCost, GetHeuristic(neighborCell, goal));
                    openSet.Add(neighborNode);
                }
                else if (tentativeGCost < neighborNode.gCost)
                {
                    // Update existing node if better path
                    neighborNode.parent = current;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = GetHeuristic(neighborCell, goal);  // hCost doesn't change, but recalculate for safety
                }
            }
        }
        // No path found
        return null;
    }

    private List<Cell> GetWalkableNeighbors(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();

        // North (up in y)
        if (!cell.north)
        {
            Cell n = grid.GetCell(cell.x, cell.y + 1);
            if (n != null) neighbors.Add(n);
        }

        // South (down in y)
        if (!cell.south)
        {
            Cell s = grid.GetCell(cell.x, cell.y - 1);
            if (s != null) neighbors.Add(s);
        }

        // East (right in x)
        if (!cell.east)
        {
            Cell e = grid.GetCell(cell.x + 1, cell.y);
            if (e != null) neighbors.Add(e);
        }

        // West (left in x)
        if (!cell.west)
        {
            Cell w = grid.GetCell(cell.x - 1, cell.y);
            if (w != null) neighbors.Add(w);
        }

        return neighbors;
    }

    private List<Cell> ReconstructPath(Node node)
    {
        List<Cell> path = new List<Cell>();
        while (node != null)
        {
            path.Add(node.cell);
            node = node.parent;
        }
        path.Reverse();  // Start to goal
        return path;
    }

    private Node GetLowestFCostNode(List<Node> nodes)
    {
        Node lowest = nodes[0];
        foreach (Node node in nodes)
        {
            if (node.fCost < lowest.fCost || (node.fCost == lowest.fCost && node.hCost < lowest.hCost))
            {
                lowest = node;
            }
        }
        return lowest;
    }

    private int GetHeuristic(Cell a, Cell b)
    {
        // Manhattan distance (no diagonals)
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
