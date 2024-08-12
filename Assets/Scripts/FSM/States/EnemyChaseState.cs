using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateManager) : base(enemy, stateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("enemy_Chase");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
        if(!enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.PatrolState);
        }

        if (enemy != null && enemy.Movement != null)
        {
            enemy.Movement.FollowTarget();
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

