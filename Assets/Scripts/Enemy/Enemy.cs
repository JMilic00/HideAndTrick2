using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding.BehaviourTrees;
using JetBrains.Annotations;

public class Enemy : PoolableObject, IDamageable, ITriggerCheckable
{
    public AttackRadius AttackRadius;
    public Animator Animator;
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public int health = 100;
    public int maxHealth;

    private Coroutine LookCoroutine;
    public const string ATTACK_TRIGGER = "Attack";
    public const string ATTACK_SHIELD_TRIGGER = "ShieldAttack";


    public EnemyStateMachine StateMachine { get; set; }
    public EnemyPatrolState PatrolState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }


    BehaviourTree tree;

    private void Awake()
    {
        if (Movement != null)
        {
            //Debug.Log("usao u movement ");
            Movement.SetEnemyContext(this);
        }
        if (AttackRadius == null && Movement == null)
        {
            //Debug.Log("EnemyMovement or AttackRadius is not assigned on " + gameObject.name);
        }

        if (AttackRadius != null)
        {
           // Debug.Log("usao u attack ");
            AttackRadius.SetEnemyContext(this);
            AttackRadius.OnAttack += OnAttack;
        }
        else
        {
            //Debug.Log("AttackRadius is not assigned on " + gameObject.name);
        }

        StateMachine = new EnemyStateMachine();

        PatrolState = new EnemyPatrolState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);


        tree = new BehaviourTree("enemy");

        // Selector node to choose between patrol, chase, and attack
        BehaviourSelector enemyStateSelector = new BehaviourSelector("enemyStateSelector");

        // Patrol sequence
        Leaf patrol = new Leaf("patrol", new PatrolStrategy(this));

        // Chase sequence
        Leaf isAggroed = new Leaf("isAggroed", new Condition(() => this.IsAggroed));
        Leaf chase = new Leaf("chase", new ActionStrategy(() => Movement.FollowTarget()));
        BehaviourSequence chaseSequence = new BehaviourSequence("chaseSequence");
        chaseSequence.AddChild(isAggroed);
        chaseSequence.AddChild(chase);

        // Attack sequence
        Leaf isWithinStrikingDistance = new Leaf("isWithinStrikingDistance", new Condition(() => this.IsWithinStrikingDistance));
        Leaf attackSword = new Leaf("Swordattack", new ActionStrategy(() => AttackRadius.Attack(ATTACK_TRIGGER)));
        Leaf attackShield = new Leaf("Shieldattack", new ActionStrategy(() => AttackRadius.Attack(ATTACK_SHIELD_TRIGGER)));

        BehaviourSequence attackSequence = new BehaviourSequence("attackSequence");
        attackSequence.AddChild(isWithinStrikingDistance);
        RandomSelector EnemyAttackType = new RandomSelector("EnemyAttackType");
        EnemyAttackType.AddChild(attackSword);
        EnemyAttackType.AddChild(attackShield);
        attackSequence.AddChild(EnemyAttackType);

        // Add sequences to selector
        enemyStateSelector.AddChild(attackSequence);
        enemyStateSelector.AddChild(chaseSequence);
        enemyStateSelector.AddChild(patrol);

        // Add selector to tree
        tree.AddChild(enemyStateSelector);
    }

    private void Start()
    {

        //StateMachine.initialize(PatrolState);
    }

    private void Update()
    {
        tree.Process();
    }

    private void FixedUpdate()
    {
        /*
        StateMachine.CurrentEnemyState.UpdatePhysics();
        */
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
        health -= Damage;
        Debug.Log("ENEMY TOOK DAMAGE: " + Damage + ", REMAINING HEALTH: " + health);
        if (health <= 0)
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

    public bool AggroedStatus()
    {
        return IsAggroed;
    }

    public bool StrikingDistanceBool()
    {
        return IsWithinStrikingDistance;
    }
}