using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private AttackRadius AttackRadius;
    [SerializeField]
    private Animator Animator;
    private Coroutine LookCoroutine;

    [SerializeField]
    private int Health = 300;

    private const string ATTACK_TRIGGER = "Attack";

    private void Awake()
    {
        AttackRadius.OnAttack += OnAttack;
    }

    private void OnAttack(IDamageable Target, string attackTrigger)
    {
        Animator.SetTrigger(attackTrigger);

        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
    }

    private IEnumerator LookAt(Transform Target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
        float time = 0;

        while (time < 3)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        Debug.Log("Player took damage: " + Damage + ", remaining health: " + Health);

        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}