using UnityEngine;
using TMPro;

public class MachineGun : MonoBehaviour
{
    [Header("Mermi Ayarları")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 25f;
    public float fireRate = 0.1f;
    public int maxAmmo = 30;
    public float reloadTime = 2f;
    public int bulletDamage = 20;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    [Header("Kamera")]
    public Camera fpsCamera;

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        Vector3 direction = fpsCamera.transform.forward;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
            rb.useGravity = false;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = bulletDamage;
        }

        currentAmmo--;
        UpdateAmmoUI();
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
        }
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
