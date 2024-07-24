using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider Collider;
    public float FieldOfView = 90f;
    public LayerMask LineOfSightLayers;

    public delegate void GainSightEvent(Player player);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(Player player);
    public LoseSightEvent OnLoseSight;

    private Coroutine CheckForLineOfSightCoroutine;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            if (!CheckLineOfSight(player))
            {
                CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            OnLoseSight?.Invoke(player);
            if (CheckForLineOfSightCoroutine != null)
            {
                StopCoroutine(CheckForLineOfSightCoroutine);
            }
        }
    }

    private bool CheckLineOfSight(Player player)
    {
        Vector3 Direction = (player.transform.position - transform.position).normalized;
        float DotProduct = Vector3.Dot(transform.forward, Direction);
        float FoV = Mathf.Cos(FieldOfView);
        //Debug.Log("DotProduct: "+ DotProduct!);
        //Debug.Log("cosinus "+ Mathf.Cos(FieldOfView));
        /* if (1 == 1)
         {
             RaycastHit Hit;
             Debug.Log("not entered yet  " + Physics.Raycast(transform.position, Direction, out Hit, Collider.radius, LineOfSightLayers));
             if (Physics.Raycast(transform.position, Direction, out Hit, Collider.radius, LineOfSightLayers))
             {
                 Debug.Log("Enter! "+ Physics.Raycast(transform.position, Direction, out Hit, Collider.radius, LineOfSightLayers));
                 if (Hit.transform.GetComponent<Player>() != null)
                 {
                     OnGainSight?.Invoke(player);
                     return true;
                 }
             }
         }
         Debug.Log("exit!");
         return false;
        */
        RaycastHit hit;
        Vector3 direction = (player.transform.position - transform.position).normalized;

        Debug.Log("Checking line of sight...");
        bool isHit = Physics.Raycast(transform.position, direction, out hit, Collider.radius, LineOfSightLayers);
        Debug.Log("Raycast result: " + isHit);

        if (isHit)
        {
            Debug.Log("Hit something: " + hit.transform.name);
            if (hit.transform.GetComponent<Player>() != null)
            {
                Debug.Log("Player in sight!");
                OnGainSight?.Invoke(player);
                return true;
            }
        }

        Debug.Log("Player not in sight.");
        return false;
    }

    private IEnumerator CheckForLineOfSight(Player player)
    {
        WaitForSeconds Wait = new WaitForSeconds(0.1f);

        while (!CheckLineOfSight(player))
        {
            yield return Wait;
        }
    }
}