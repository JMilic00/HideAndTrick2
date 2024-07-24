using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject, IDamageable
{
    public AttackRadius AttackRadius;
    public Animator Animator;
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public int Health = 100;

    private Coroutine LookCoroutine;
    private const string ATTACK_TRIGGER = "Attack";

    private void Awake()
    {
        if (AttackRadius != null)
        {
            AttackRadius.OnAttack += OnAttack;
        }
        else
        {
            Debug.LogError("AttackRadius is not assigned on " + gameObject.name);
        }
        //here I encoutered some problems with NullReferenceException: Object reference not set to an instance of an object
    }

    private void OnAttack(IDamageable Target)
    {
        Animator.SetTrigger(ATTACK_TRIGGER);

        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
    }

    private IEnumerator LookAt(Transform Target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }


    public void TakeDamage(int Damage)
    {
        Health -= Damage;

        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}