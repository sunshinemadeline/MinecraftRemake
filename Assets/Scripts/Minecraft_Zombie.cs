using System.Collections.Generic;
using UnityEngine;

public class Minecraft_Zombie : MonoBehaviour
{
    public Minecraft_Pathfinder pathfinder;
    public Transform target;

    public float speed = 3f;

    private List<Vector3Int> path;
    private int currentPathIndex = 0;

    void Start()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, 1f);
    }

    void Update()
    {
        FollowPath();
    }

    void UpdatePath()
    {
        if (pathfinder == null || target == null) return;

        Vector3Int start = Vector3Int.RoundToInt(transform.position);
        Vector3Int goal = Vector3Int.RoundToInt(target.position);

        path = pathfinder.FindPath(start, goal);
        currentPathIndex = 0;
    }

    void FollowPath()
    {
        if (path == null || path.Count == 0) return;
        if (currentPathIndex >= path.Count) return;

        Vector3 targetPos = path[currentPathIndex] + new Vector3(0.5f, 0f, 0.5f);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentPathIndex++;
        }
    }
}