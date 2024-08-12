using UnityEngine;

public class EnemyBaseState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateManager;

    public EnemyBaseState(Enemy enemy, EnemyStateMachine stateManager)
    {
        this.enemy = enemy;
        this.stateManager = stateManager;
    }

    public virtual void EnterState() { }

    public virtual void UpdateLogic() { }

    public virtual void UpdatePhysics() { }

    public virtual void ExitState() { }
}
