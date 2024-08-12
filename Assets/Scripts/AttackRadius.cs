using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackRadius : MonoBehaviour
{
    public SphereCollider Collider;
    private List<IDamageable> Damageables = new List<IDamageable>();
    public int Damage = 10;
    public float AttackDelay= 1.0f; // Cooldown in seconds
    private float lastAttackTime = 0f;
    public delegate void AttackEvent(IDamageable Target);
    public AttackEvent OnAttack;
    private Enemy _enemy;

    public void SetEnemyContext(Enemy enemy)
    {
        _enemy = enemy;
        if (_enemy != null)
        {
            Debug.Log("kontekst je u lineu za attack ");
        }
        else
        {
            Debug.LogError("LineOfSightChecker is not assigned on " + gameObject.name);
        }
    }
    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
                _enemy.SetStrikingDistanceBool(true);
                Damageables.Add(damageable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Damageables.Remove(damageable);
            if (Damageables.Count == 0 && _enemy != null)
            {
                _enemy.SetStrikingDistanceBool(false);
            }
        }
        else
        {
            Debug.LogWarning("Null damageable detected in OnTriggerExit for " + other.name);
        }
    }

    public void Attack()
    {
        if (Time.time - lastAttackTime < AttackDelay) return; // Check if cooldown has elapsed
        lastAttackTime = Time.time; // Update last attack time

        IDamageable closestDamageable = null;
        float closestDistance = float.MaxValue;

        foreach (var damageable in Damageables)
        {
            if (damageable == null) continue;

            Transform damageableTransform = damageable.GetTransform();
            float distance = Vector3.Distance(transform.position, damageableTransform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDamageable = damageable;
            }
        }

        if (closestDamageable != null)
        {
            OnAttack?.Invoke(closestDamageable);
            closestDamageable.TakeDamage(Damage);
        }

        Damageables.RemoveAll(DisabledDamageables);
    }

    private bool DisabledDamageables(IDamageable Damageable)
    {
        return Damageable != null && !Damageable.GetTransform().gameObject.activeSelf;
    }
}