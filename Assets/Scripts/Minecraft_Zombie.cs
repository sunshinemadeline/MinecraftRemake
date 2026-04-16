using System.Collections.Generic;
using UnityEngine;

public class Minecraft_Zombie : MonoBehaviour
{
    public enum AIState
    {
        Wandering,
        Chasing
    }

    public Minecraft_Pathfinder pathfinder;
    public Transform target;

    public float speed = 3f;

    public float chaseDistance = 6f;
    public float wanderRadius = 5f;
    public float repathInterval = 1f;

    public AIState currentState = AIState.Wandering;

    private List<Vector3Int> path;
    private int currentPathIndex = 0;
    private float repathTimer = 0f;
    private Vector3Int currentWanderGoal;

    void Start()
    {
        PickNewWanderGoal();
        UpdatePath();
    }

    void Update()
    {
        UpdateState();
        UpdatePathTimer();
        FollowPath();
    }

    void UpdateState()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= chaseDistance)
        {
            if (currentState != AIState.Chasing)
            {
                currentState = AIState.Chasing;
                UpdatePath();
            }
        }
        else
        {
            if (currentState != AIState.Wandering)
            {
                currentState = AIState.Wandering;
                PickNewWanderGoal();
                UpdatePath();
            }
        }
    }

    void UpdatePathTimer()
    {
        repathTimer += Time.deltaTime;

        if (repathTimer >= repathInterval)
        {
            repathTimer = 0f;

            if (currentState == AIState.Wandering)
            {
                if (path == null || currentPathIndex >= path.Count)
                {
                    PickNewWanderGoal();
                }
            }

            UpdatePath();
        }
    }

    void UpdatePath()
    {
        if (pathfinder == null) return;

        Vector3Int start = Vector3Int.RoundToInt(transform.position);

        if (currentState == AIState.Chasing)
        {
            if (target == null) return;

            Vector3Int goal = Vector3Int.RoundToInt(target.position);
            goal.y -= 4;
            path = pathfinder.FindPath(start, goal);
            currentPathIndex = 0;
        }
        else if (currentState == AIState.Wandering)
        {
            path = pathfinder.FindPath(start, currentWanderGoal);
            currentPathIndex = 0;

            if (path == null || path.Count == 0)
            {
                PickNewWanderGoal();
                path = pathfinder.FindPath(start, currentWanderGoal);
                currentPathIndex = 0;
            }
        }
    }

    void FollowPath()
    {
        if (path == null || path.Count == 0) return;
        if (currentPathIndex >= path.Count) return;

        Vector3 targetPos = path[currentPathIndex] + new Vector3(0.5f, 0f, 0.5f);
        targetPos.y = transform.position.y;

        Vector3 moveDir = (targetPos - transform.position);
        if (moveDir != Vector3.zero)
        {
            Vector3 lookDir = new Vector3(moveDir.x, 0f, moveDir.z);
            if (lookDir != Vector3.zero)
            {
                transform.forward = lookDir.normalized;
            }
        }

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

    void PickNewWanderGoal()
    {
        if (pathfinder == null || pathfinder.worldQuery == null)
            return;

        Vector3Int currentPos = Vector3Int.RoundToInt(transform.position);

        for (int i = 0; i < 20; i++)
        {
            int randomX = Random.Range(-Mathf.RoundToInt(wanderRadius), Mathf.RoundToInt(wanderRadius) + 1);
            int randomZ = Random.Range(-Mathf.RoundToInt(wanderRadius), Mathf.RoundToInt(wanderRadius) + 1);

            Vector3Int candidate = new Vector3Int(
                currentPos.x + randomX,
                currentPos.y,
                currentPos.z + randomZ
            );

            if (pathfinder.worldQuery.IsWalkable(candidate))
            {
                currentWanderGoal = candidate;
                return;
            }
        }

        currentWanderGoal = currentPos;
    }

    void OnDrawGizmos()
    {
        if (path == null || path.Count == 0) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = path[i] + new Vector3(0.5f, 0.5f, 0.5f);

            if (i == currentPathIndex)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawCube(pos, new Vector3(0.3f, 0.3f, 0.3f));

            if (i > 0)
            {
                Vector3 prevPos = path[i - 1] + new Vector3(0.5f, 0.5f, 0.5f);
                Gizmos.DrawLine(prevPos, pos);
            }
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}