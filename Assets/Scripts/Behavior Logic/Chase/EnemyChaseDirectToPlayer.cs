using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase-Direct", menuName = "Enemy Logic/Chase Logic/Direct Chase")]
public class EnemyChaseDirectToPlayer : EnemyChaseSOBase
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
            enemy.Movement.FollowTarget();
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
