using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPatrol", menuName = "Enemy Logic/Patrol Logic/Classic Partrol")]
public class EnemyPatrol : EnemyPatrolSOBase
{
    public override void DoEnterState()
    {
        base.DoEnterState();
    }

    public override void DoExitState()
    {
        base.DoExitState();
    }

    public override void DoUpdateLogic()
    {
        base.DoUpdateLogic();
        if (enemy != null && enemy.Movement != null)
        {
            enemy.Movement.DoPatrolMotion();
        }
    }

    public override void DoUpdatePhysics()
    {
        base.DoUpdatePhysics();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }
}
