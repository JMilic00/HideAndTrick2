using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private NavMeshAgent Agent;
    [SerializeField]
    private Animator Animator;

    private const string IsWalking = "IsWalking";
    private RaycastHit[] RaycastHit = new RaycastHit[1];

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse0)) 
        { 
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            if(Physics.RaycastNonAlloc(ray, RaycastHit) > 0)
            {
                Agent.SetDestination(RaycastHit[0].point);
            }
        }

        Animator.SetBool(IsWalking, Agent.velocity.magnitude > 0.5f);
    }
}

