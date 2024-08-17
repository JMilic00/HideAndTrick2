using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackShield : EnemyBaseState
{
    public EnemyAttackShield(Enemy enemy, EnemyStateMachine stateManager) : base(enemy, stateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Playing Attack Shield");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (enemy != null && enemy.Movement != null)
        {
            enemy.Movement.FollowTarget();
            enemy.AttackRadius.Attack(Enemy.ATTACK_SHIELD_TRIGGER);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
