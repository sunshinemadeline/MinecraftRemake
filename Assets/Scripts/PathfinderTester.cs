using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderTester : MonoBehaviour
{
    public Minecraft_Pathfinder pathfinder;
    public Vector3Int start = new Vector3Int(0, 3, 0);
    public Vector3Int goal = new Vector3Int(5, 3, 5);

    private List<Vector3Int> path;

    IEnumerator Start()
    {
        yield return null; // wait 1 frame so world generation finishes

        path = pathfinder.FindPath(start, goal);

        if (path == null)
        {
            Debug.Log("No path found.");
        }
        else
        {
            Debug.Log("Path found with " + path.Count + " nodes.");
        }
    }

    void OnDrawGizmos()
    {
        if (path == null || path.Count == 0)
            return;

        Gizmos.color = Color.blue;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 drawPos = path[i] + new Vector3(0.5f, 0.5f, 0.5f);
            Gizmos.DrawCube(drawPos, new Vector3(0.3f, 0.3f, 0.3f));

            if (i > 0)
            {
                Vector3 prevPos = path[i - 1] + new Vector3(0.5f, 0.5f, 0.5f);
                Gizmos.DrawLine(prevPos, drawPos);
            }
        }
    }
}