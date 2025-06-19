using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyBehavior : MonoBehaviour
{
    private EnemyData data;
    private NavMeshAgent agent;
    private GameObject target;
    private Animator animator;
    private HealthComponent health;

    private float checkInterval = 1f;
    private float checkTimer;
    private float lastAttackTime;

    private enum AnimationState { Idle, Walk, Attack, Die }
    private AnimationState currentState = AnimationState.Idle;

    public event Action OnDeath;

    public void Initialize(EnemyData enemyData)
    {
        data = enemyData;

        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.speed = data.Speed;

        animator = GetComponent<Animator>();

        health = GetComponent<HealthComponent>();
        if (health == null)
            health = gameObject.AddComponent<HealthComponent>();
        health.SetMaxHealth(data.Health);
        health.OnDeath -= HandleDeath;
        health.OnDeath += HandleDeath;

        FindClosestTarget();
        SetAnimationState(AnimationState.Walk);
    }

    private void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            checkTimer = checkInterval;
            FindClosestTarget();
        }

        if (target == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= data.AttackRange)
        {
            if (Time.time - lastAttackTime >= data.AttackCooldown)
            {
                AttackTarget();
                lastAttackTime = Time.time;
            }

            agent.isStopped = true;
            SetAnimationState(AnimationState.Attack);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            SetAnimationState(AnimationState.Walk);
        }
    }

    private void FindClosestTarget()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");

        GameObject closestTurret = null;
        float closestDistance = float.MaxValue;

        foreach (var turret in turrets)
        {
            float dist = Vector3.Distance(transform.position, turret.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTurret = turret;
            }
        }

        GameObject newTarget = closestTurret != null ? closestTurret : GameObject.FindGameObjectWithTag("Frog");

        if (newTarget != target)
        {
            target = newTarget;
            if (agent != null && target != null)
                agent.SetDestination(target.transform.position);
        }
        else if (target != null && agent != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private void AttackTarget()
    {
        var healthComponent = target.GetComponent<HealthComponent>();
        if (healthComponent != null)
            healthComponent.TakeDamage(data.Damage);
    }

    public void TakeDamage(float damage)
    {
        health?.TakeDamage(damage);
    }

    private void HandleDeath()
    {
        Debug.Log($"{gameObject.name} HandleDeath called. Adding reward {data.Reward}");

        EconomyManager.Instance.AddCoins(data.Reward);
        OnDeath?.Invoke();

        SetAnimationState(AnimationState.Die);

        agent.enabled = false;
        enabled = false;

        Destroy(gameObject, 3f);
    }

    private void SetAnimationState(AnimationState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (newState)
        {
            case AnimationState.Idle:
                animator?.SetBool("IsMoving", false);
                animator?.SetBool("IsAttacking", false);
                break;
            case AnimationState.Walk:
                animator?.SetBool("IsMoving", true);
                animator?.SetBool("IsAttacking", false);
                break;
            case AnimationState.Attack:
                animator?.SetBool("IsMoving", false);
                animator?.SetBool("IsAttacking", true);
                break;
            case AnimationState.Die:
                animator?.SetTrigger("Die");
                animator?.SetBool("IsMoving", false);
                animator?.SetBool("IsAttacking", false);
                break;
        }
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnDeath -= HandleDeath;
    }
}
