using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private ObjectDatabaseSO objectDatabase;
    [SerializeField] private int turretObjectId;

    private ObjectData objectData;

    [SerializeField] private float shootRange = 10f;
    [SerializeField] private float shootCooldown = 1f;

    private float cooldownTimer = 0f;

    private EnemyBehavior currentTarget;

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
        float closestDist = shootRange;

        foreach (var enemyGO in enemies)
        {
            EnemyBehavior enemy = enemyGO.GetComponent<EnemyBehavior>();
            if (enemy == null)
                continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= shootRange && dist < closestDist)
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

        GameObject projGO = Instantiate(objectData.ProjectilePrefab, transform.position, Quaternion.identity);
        Projectile proj = projGO.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetTarget(target);
            proj.SetDamage(objectData.Damage);
        }
    }
}
