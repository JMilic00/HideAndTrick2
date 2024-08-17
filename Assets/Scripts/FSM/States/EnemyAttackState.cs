using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private EnemyAttackOneHand enemyAttackOneHand;
    private EnemyAttackShield enemyAttackShield;
    private EnemyBaseState currentSubState;

    private System.Random random;
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateManager) : base(enemy, stateManager)
    {
        enemyAttackOneHand = new EnemyAttackOneHand(enemy, stateManager);
        enemyAttackShield = new EnemyAttackShield(enemy, stateManager);

        random = new System.Random();
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("enemy_Attack");

        // Randomly select an attack animation state
        int randomAnimation = random.Next(2);
        if (randomAnimation == 0)
        {
            currentSubState = enemyAttackOneHand;
        }
        else
        {
            currentSubState = enemyAttackShield;
        }

        currentSubState.EnterState();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (!enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
        currentSubState.UpdateLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
}