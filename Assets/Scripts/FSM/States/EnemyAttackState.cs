using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateManager) : base(enemy, stateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("enemy_Attack");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (!enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }

        if (enemy != null && enemy.Movement != null)
        {
            enemy.Movement.FollowTarget();
            enemy.AttackRadius.Attack();
        }
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