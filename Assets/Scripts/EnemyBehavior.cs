using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private EnemyData data;
    private NavMeshAgent agent;
    private GameObject target;

    private float checkInterval = 1f;
    private float checkTimer;

    private float lastAttackTime;

    public void Initialize(EnemyData enemyData)
    {
        data = enemyData;
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.speed = data.Speed;

        FindClosestTarget();
    }

    private void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            checkTimer = checkInterval;
            FindClosestTarget();
        }

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= data.AttackRange)
            {
                if (Time.time - lastAttackTime >= data.AttackCooldown)
                {
                    AttackTarget();
                    lastAttackTime = Time.time;
                }
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
            }
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
            {
                agent.SetDestination(target.transform.position);
            }
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
        {
            healthComponent.TakeDamage(data.Damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            Destroy(gameObject);
        }
    }
}
