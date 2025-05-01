using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Silahlar ve UI")]
    public List<GameObject> weapons;
    public TMP_Text ammoTextUI;

    private int currentWeaponIndex = 0;

    void Start()
    {
        ActivateWeapon(currentWeaponIndex);
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            ActivateWeapon(currentWeaponIndex);
        }
        else if (scroll < 0f)
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
                currentWeaponIndex = weapons.Count - 1;

            ActivateWeapon(currentWeaponIndex);
        }
    }

    void ActivateWeapon(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == index);
        }

        UpdateAmmoUI(weapons[index]);
        Debug.Log("Selected Weapon: " + weapons[index].name);
    }

    void UpdateAmmoUI(GameObject weapon)
    {
        if (ammoTextUI == null) return;
        
        var machineGun = weapon.GetComponent<MachineGun>();
        if (machineGun != null)
        {
            ammoTextUI.text = machineGun.GetCurrentAmmo() + " / " + machineGun.maxAmmo;
            return;
        }

        var pistol = weapon.GetComponent<Weapon>();
        if (pistol != null)
        {
            ammoTextUI.text = pistol.GetCurrentAmmo() + " / " + pistol.maxAmmo;
            return;
        }
        
        var melee = weapon.GetComponent<MeleeWeapon>();
        if (melee != null)
        {
            ammoTextUI.text = "∞ / ∞";
            return;
        }
        
        ammoTextUI.text = "- / -";
    }
}