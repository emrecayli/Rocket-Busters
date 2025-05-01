using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 25;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        
        Destroy(gameObject);
    }
}