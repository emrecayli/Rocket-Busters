using UnityEngine;
using TMPro;

public class MeleeWeapon : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    public int damage = 30;
    public LayerMask hitLayers;

    [Header("UI")]
    public TMP_Text ammoText;

    void Start()
    {
        UpdateAmmoUI();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        Debug.Log("Knife Attack");

        RaycastHit hit;
        Vector3 origin = transform.position + transform.forward * 0.5f; // Ucundan başlat
        if (Physics.Raycast(origin, transform.forward, out hit, attackRange, hitLayers))
        {
            Debug.Log("Knife hit: " + hit.collider.name);

            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Damage dealt: " + damage);
            }
        }
        else
        {
            Debug.Log("No hit detected.");
        }
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "∞ / ∞";
        }
    }
}