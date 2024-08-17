using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;


    public virtual void Initialize(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        this.enemy = enemy;
        transform = gameObject.transform;
    }

    public virtual void DoEnterState() { }

    public virtual void DoUpdateLogic() 
    {
        if (enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
    }

    public virtual void DoUpdatePhysics() { }

    public virtual void DoExitState() { }
}
