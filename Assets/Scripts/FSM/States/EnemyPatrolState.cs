using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    
    public EnemyPatrolState(Enemy enemy, EnemyStateMachine stateManager) : base(enemy, stateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("enemy_patrol");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }

        if (enemy != null && enemy.Movement != null)
        {
            enemy.Movement.DoPatrolMotion();
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
