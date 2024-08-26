using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider Collider;
    public float FieldOfView = 90f; // Field of view in degrees
    public LayerMask LineOfSightLayers;
    private Coroutine CheckForLineOfSightCoroutine;
    private Enemy _enemy;
    public void SetEnemyContext(Enemy enemy)
    {
        _enemy = enemy;
        if ( _enemy != null)
        {
            Debug.Log("kontekst je u lineu za movement ");
        }
        else
        {
            Debug.LogError("LineOfSightChecker is not assigned on " + gameObject.name);
        }
    }
    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (_enemy == null)
        {
            Debug.LogError("Enemy context is not set for " + gameObject.name);
            return;
        }
        if (other.TryGetComponent<Player>(out Player player))
        {
            Debug.Log("playera smo nasli ");
            if (!CheckLineOfSight(player))
            {
                CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(player));
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (_enemy == null)
        {
            Debug.LogError("Enemy context is not set for " + gameObject.name);
            return;
        }

        if (other.TryGetComponent<Player>(out Player player))
        {
            _enemy.SetAggroedStatus(false);
            if (CheckForLineOfSightCoroutine != null)
            {
                StopCoroutine(CheckForLineOfSightCoroutine);
                CheckForLineOfSightCoroutine = null;
            }
        }
    }

    public bool CheckLineOfSight(Player player)
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        RaycastHit hit;
        Debug.DrawRay(transform.position, directionToPlayer * distanceToPlayer, Color.red, 1.0f);

        bool isHit = Physics.Raycast(transform.position, directionToPlayer, out hit, distanceToPlayer, LineOfSightLayers);

        if (isHit && hit.transform.GetComponent<Player>() != null)
        {
            _enemy.SetAggroedStatus(true);
            //Debug.Log(_enemy.IsAggroed);
            return true;
        }

        return false;
    }

    private IEnumerator CheckForLineOfSight(Player player)
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            if (CheckLineOfSight(player))
            {
                CheckForLineOfSightCoroutine = null;
                yield break;
            }
            yield return wait;
        }
    }
}