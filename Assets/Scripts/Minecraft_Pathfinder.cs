using System.Collections.Generic;
using UnityEngine;

public class Minecraft_Pathfinder : MonoBehaviour
{
    public Minecraft_WorldQuery worldQuery;

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        if (!worldQuery.IsWalkable(start) || !worldQuery.IsWalkable(goal))
        {
            Debug.LogWarning("Start or goal is not walkable.");
            return null;
        }

        PriorityQueue<Node> openSet = new PriorityQueue<Node>();
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, float> bestGCost = new Dictionary<Vector3Int, float>();

        Node startNode = new Node(start, 0f, Heuristic(start, goal), null);
        openSet.Enqueue(startNode);
        bestGCost[start] = 0f;

        while (openSet.Count > 0)
        {
            Node current = openSet.Dequeue();

            if (closedSet.Contains(current.position))
                continue;

            if (current.position == goal)
            {
                return ReconstructPath(current);
            }

            closedSet.Add(current.position);

            foreach (Vector3Int neighborPos in GetNeighbors(current.position))
            {
                if (!worldQuery.IsWalkable(neighborPos))
                    continue;

                if (closedSet.Contains(neighborPos))
                    continue;

                float stepCost = 1f;

                if (neighborPos.y > current.position.y)
                {
                    stepCost = 1.5f; // stepping up is harder
                }

                float newGCost = current.gCost + stepCost;

                if (bestGCost.ContainsKey(neighborPos) && newGCost >= bestGCost[neighborPos])
                    continue;

                bestGCost[neighborPos] = newGCost;

                Node neighborNode = new Node(
                    neighborPos,
                    newGCost,
                    Heuristic(neighborPos, goal),
                    current
                );

                openSet.Enqueue(neighborNode);
            }
        }

        return null;
    }

    private float Heuristic(Vector3Int a, Vector3Int b)
    {
        return Vector3.Distance(a, b);
    }

    private List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        Vector3Int[] directions =
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 1),
            new Vector3Int(0, 0, -1)
        };

        foreach (var dir in directions)
        {
            Vector3Int forward = pos + dir;

            // 1️⃣ same level
            if (worldQuery.IsWalkable(forward))
            {
                neighbors.Add(forward);
                continue;
            }

            // 2️⃣ step UP (like climbing a block)
            Vector3Int up = forward + Vector3Int.up;
            if (worldQuery.IsWalkable(up))
            {
                neighbors.Add(up);
                continue;
            }

            // 3️⃣ step DOWN (like walking off edge)
            Vector3Int down = forward + Vector3Int.down;
            if (worldQuery.IsWalkable(down))
            {
                neighbors.Add(down);
            }
        }

        return neighbors;
    }
    private List<Vector3Int> ReconstructPath(Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node current = endNode;

        while (current != null)
        {
            path.Add(current.position);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }
}