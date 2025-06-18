using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private ObjectDatabaseSO objectDatabase;
    [SerializeField] private int turretObjectId;

    private ObjectData objectData;
    private HealthComponent health;

    private float shootCooldown;
    private float cooldownTimer = 0f;

    private EnemyBehavior currentTarget;
    private Transform firePoint;

    private void Start()
    {
        if (objectDatabase != null)
        {
            objectData = objectDatabase.objectData.Find(o => o.ID == turretObjectId);
            if (objectData == null)
                Debug.LogWarning($"ObjectData with ID {turretObjectId} not found in database.");
        }
        else
        {
            Debug.LogWarning("ObjectDatabaseSO not assigned in Turret!");
        }

        health = GetComponent<HealthComponent>();
        if (health == null)
        {
            health = gameObject.AddComponent<HealthComponent>();
        }

        health.SetMaxHealth(objectData != null ? objectData.Health : 100f);
        health.OnDeath += HandleDeath;

        shootCooldown = objectData != null && objectData.AttackRate > 0f ? 1f / objectData.AttackRate : 1f;

        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint not found as a child of Turret, projectiles will spawn at turret position.");
        }
    }

    private void HandleDeath()
    {
        Debug.Log($"{gameObject.name} Turret is dead.");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeath -= HandleDeath;
        }
    }

    private void Update()
    {
        if (objectData == null)
            return;

        cooldownTimer -= Time.deltaTime;

        currentTarget = FindClosestEnemyInRange();

        if (currentTarget != null)
        {
            RotateTowards(currentTarget.transform.position);

            if (cooldownTimer <= 0f)
            {
                Shoot(currentTarget);
                cooldownTimer = shootCooldown;
            }
        }
    }

    private EnemyBehavior FindClosestEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyBehavior closestEnemy = null;
        float closestDist = objectData.ShootRange;

        foreach (var enemyGO in enemies)
        {
            EnemyBehavior enemy = enemyGO.GetComponent<EnemyBehavior>();
            if (enemy == null)
                continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= objectData.ShootRange && dist < closestDist)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 5f;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Shoot(EnemyBehavior target)
    {
        if (objectData.ProjectilePrefab == null)
        {
            Debug.LogWarning("ProjectilePrefab not set on turret ObjectData!");
            return;
        }

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Quaternion spawnRot = firePoint != null ? firePoint.rotation : Quaternion.identity;

        GameObject projGO = Instantiate(objectData.ProjectilePrefab, spawnPos, spawnRot);
        Projectile proj = projGO.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetTarget(target);
            proj.SetDamage(objectData.Damage);
        }
    }
}
