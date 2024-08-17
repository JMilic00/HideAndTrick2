using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject, IDamageable, ITriggerCheckable
{
    public AttackRadius AttackRadius;
    public Animator Animator;
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public int Health = 100;

    private Coroutine LookCoroutine;
    public const string ATTACK_TRIGGER = "Attack";
    public const string ATTACK_SHIELD_TRIGGER = "ShieldAttack";

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyPatrolState PatrolState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    private void Awake()
    {
        if (Movement != null)
        {
            Debug.LogError("usao u movement ");
            Movement.SetEnemyContext(this);
        }
        if (AttackRadius != null && Movement != null)
        {
            Debug.LogError("EnemyMovement or AttackRadius is not assigned on " + gameObject.name);
        }

        if (AttackRadius != null)
        {
            Debug.LogError("usao u attack ");
            AttackRadius.SetEnemyContext(this);
            AttackRadius.OnAttack += OnAttack;
        }
        else
        {
            Debug.LogError("AttackRadius is not assigned on " + gameObject.name);
        }

        StateMachine = new EnemyStateMachine();

        PatrolState = new EnemyPatrolState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.initialize(PatrolState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.UpdateLogic();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.UpdatePhysics();
    }

    private void OnAttack(IDamageable Target, string attackTrigger)
    {
        if (attackTrigger == ATTACK_SHIELD_TRIGGER)
        {
            Debug.Log("SHIELD UDARA");
        }
        Animator.SetTrigger(attackTrigger);

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
        Debug.Log("ENEMY TOOK DAMAGE: " + Damage + ", REMAINING HEALTH: " + Health);

        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SetAggroedStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetStrikingDistanceBool(bool isStrikingDistance)
    {
        IsWithinStrikingDistance = isStrikingDistance;
    }
}