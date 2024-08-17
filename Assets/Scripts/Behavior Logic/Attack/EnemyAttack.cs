using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Enemy Logic/Attack Logic/Sword Attack")]
public class EnemyAttack : EnemyAttackSOBase
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
            //enemy.AttackRadius.Attack();
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
