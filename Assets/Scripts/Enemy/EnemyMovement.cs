using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Player;
    public EnemyLineOfSightChecker LineOfSightChecker;
    public NavMeshTriangulation Triangulation;
    public float UpdateRate = 0.1f;
    private NavMeshAgent Agent;
    private Enemy _enemy;

    [SerializeField]
    private Animator Animator = null;

    private const string IsWalking = "IsWalking";

    public float IdleLocationRadius = 4f;
    public float IdleMovespeedMultiplier = 0.5f;
    public Vector3[] Waypoints = new Vector3[4];
    [SerializeField]
    public int WaypointIndex = 0;
    public void SetEnemyContext(Enemy enemy)
    {
        _enemy = enemy;
        if (LineOfSightChecker != null && _enemy != null)
        {
            Debug.Log("usao u lineofsightchecker ");
            LineOfSightChecker.SetEnemyContext(_enemy);
        }
        else
        {
            Debug.LogError("LineOfSightChecker is not assigned on " + gameObject.name);
        }
    }
    private void Awake()
    {

        Agent = GetComponent<NavMeshAgent>();
    }

    public void Spawn()
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {
            NavMeshHit Hit;
            if (NavMesh.SamplePosition(Triangulation.vertices[Random.Range(0, Triangulation.vertices.Length)], out Hit, 2f, Agent.areaMask))
            {
                Waypoints[i] = Hit.position;
            }
            else
            {
                Debug.LogError("Unable to find position for navmesh near Triangulation vertex!");
            }
        }
    }

    private void Update()
    {
        Animator.SetBool(IsWalking, Agent.velocity.magnitude > 0.1f);
    }

    public void DoIdleMotion()
    {

        Agent.speed *= IdleMovespeedMultiplier;

            if (!Agent.enabled || !Agent.isOnNavMesh)
            {
                Debug.Log("Agent not found");
                return; 
            }
            else if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                Vector2 point = Random.insideUnitCircle * IdleLocationRadius;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(Agent.transform.position + new Vector3(point.x, 0, point.y), out hit, 2f, Agent.areaMask))
                {
                    Agent.SetDestination(hit.position);
                    return;
                }
                return;
            }
        
    }

    public void DoPatrolMotion()
    {
        if (!Agent.enabled || !Agent.isOnNavMesh)
        {
            Debug.Log("Agent not found");
            return;
        }
        Agent.SetDestination(Waypoints[WaypointIndex]);

            if (Agent.isOnNavMesh && Agent.enabled && Agent.remainingDistance <= Agent.stoppingDistance)
            {
                WaypointIndex++;
            
                if (WaypointIndex >= Waypoints.Length)
                {
                    WaypointIndex = 0;
                    return;
                }

                Agent.SetDestination(Waypoints[WaypointIndex]);
                Debug.Log(WaypointIndex);
        }
            return;
        
    }

    public void FollowTarget()
    {
       
        if (Agent.enabled)
        {
            Agent.SetDestination(Player.transform.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {
            Gizmos.DrawWireSphere(Waypoints[i], 0.25f);
            if (i + 1 < Waypoints.Length)
            {
                Gizmos.DrawLine(Waypoints[i], Waypoints[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(Waypoints[i], Waypoints[0]);
            }
        }

    }
}