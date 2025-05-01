using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public float viewAngle = 120f;
    public float viewDistance = 10f;
    public float moveSpeed = 2f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;

    private float lastAttackTime;
    private Transform player;
    private bool canSeePlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        canSeePlayer = (angle < viewAngle / 2f) && (distanceToPlayer < viewDistance);

        if (canSeePlayer)
        {
            directionToPlayer.y = 0f;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            
            if (distanceToPlayer <= attackRange)
            {
                TryAttack();
            }
            else
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log("Enemy melee attack!");

            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
        }
    }
}