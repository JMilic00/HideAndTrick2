using System.Collections;
using UnityEngine;

public class EnemyAttackOneHand : EnemyBaseState
{
    public EnemyAttackOneHand(Enemy enemy, EnemyStateMachine stateManager) : base(enemy, stateManager)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Playing Attack One Hand");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (enemy != null && enemy.Movement != null)
        {
            enemy.Movement.FollowTarget();
            enemy.AttackRadius.Attack(Enemy.ATTACK_TRIGGER);
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
        /*
        private string attackTrigger1;
        private string attackTrigger2;
        private Coroutine lookCoroutine;
        private Coroutine attackCoroutine;

        public EnemyAttackOneHand(Enemy enemy, EnemyStateMachine stateManager) : base(enemy, stateManager)
        {
            this.attackTrigger1 = attackTrigger1;
            this.attackTrigger2 = attackTrigger2;
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Entering Attack SubState");

            // Start the attack coroutine
            attackCoroutine = enemy.StartCoroutine(AttackRoutine());

            // Start "look at" behavior if there is a valid target
            IDamageable target = GetClosestDamageable();
            if (target != null)
            {
                if (lookCoroutine != null)
                {
                    enemy.StopCoroutine(lookCoroutine);
                }
                lookCoroutine = enemy.StartCoroutine(LookAt(target.GetTransform()));
            }
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            // If out of striking distance, transition back to chase state
            if (!enemy.IsWithinStrikingDistance)
            {
                stateManager.ChangeState(enemy.ChaseState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            Debug.Log("Exiting Attack SubState");

            // Stop the attack and look coroutines when exiting the state
            if (attackCoroutine != null)
            {
                enemy.StopCoroutine(attackCoroutine);
            }

            if (lookCoroutine != null)
            {
                enemy.StopCoroutine(lookCoroutine);
            }
        }

        private IEnumerator AttackRoutine()
        {
            while (enemy.IsWithinStrikingDistance)
            {
                // Randomly choose between the two attack animations
                string chosenTrigger = Random.value > 0.5f ? attackTrigger1 : attackTrigger2;
                enemy.Animator.SetTrigger(chosenTrigger);

                // Wait for the attack delay before the next attack
                yield return new WaitForSeconds(enemy.AttackRadius.AttackDelay);
            }
        }

        private IEnumerator LookAt(Transform target)
        {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - enemy.transform.position);
            float time = 0;

            while (time < 1)
            {
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, time);
                time += Time.deltaTime * 2;
                yield return null;
            }

            enemy.transform.rotation = lookRotation;
        }

        private IDamageable GetClosestDamageable()
        {
            float closestDistance = float.MaxValue;
            IDamageable closestDamageable = null;

            foreach (var damageable in enemy.AttackRadius.GetDamageables())
            {
                if (damageable == null) continue;

                float distance = Vector3.Distance(enemy.transform.position, damageable.GetTransform().position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = damageable;
                }
            }

            return closestDamageable;
        }
        */