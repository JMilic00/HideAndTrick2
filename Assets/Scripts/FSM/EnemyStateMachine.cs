using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyBaseState CurrentEnemyState { get; set; }
    public void initialize(EnemyBaseState startingState)
    {
        CurrentEnemyState = startingState;
        CurrentEnemyState.EnterState();
    }

    public void ChangeState(EnemyBaseState newState)
    {
        CurrentEnemyState.ExitState(); 
        CurrentEnemyState = newState;
        CurrentEnemyState.EnterState();
    }
}
