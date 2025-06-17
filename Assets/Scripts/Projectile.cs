using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private EnemyBehavior target;
    private float damage;

    public void SetTarget(EnemyBehavior enemy)
    {
        target = enemy;
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        float step = speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) <= step)
        {
            HitTarget();
        }
        else
        {
            transform.position += dir * step;
            transform.LookAt(target.transform.position);
        }
    }

    private void HitTarget()
    {
        var health = target.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Debug.Log($"Projectile hit {target.name} dealing {damage} damage.");
        }
        Destroy(gameObject);
    }

}
