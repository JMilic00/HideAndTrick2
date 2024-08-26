using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.BehaviourTrees
{
    public interface IStrategy
    {
        Node.Status Process();
        void Reset()
        {
            //Noop
        }
    }

    public class ActionStrategy : IStrategy
    {
        readonly Action doSomething;
        public ActionStrategy(Action doSomething)
        {
            this.doSomething = doSomething;
        }

        public Node.Status Process()
        {
            doSomething();
            return Node.Status.Success;
        }
    }

    public class Condition : IStrategy
    {
        readonly Func<bool> condition;

        public Condition(Func<bool> condition)
        {
            this.condition = condition;
        }

        public Node.Status Process()
        {
            return condition() ? Node.Status.Success : Node.Status.Failure;
        }
    }


    public class PatrolStrategy : IStrategy
    {
        readonly Enemy enemy;

        public PatrolStrategy(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public Node.Status Process()
        {
            if (enemy.IsAggroed)
            {
                return Node.Status.Success;
            }

            // Perform the patrol motion
            enemy.Movement.DoPatrolMotion();

            // Patrol is ongoing, so return Running to indicate the action is in progress.
            return Node.Status.Running;
        }
    }
}
