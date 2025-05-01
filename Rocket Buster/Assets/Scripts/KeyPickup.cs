using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public string keyID = "Level1Key";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeyInventory inventory = other.GetComponent<KeyInventory>();
            if (inventory != null)
            {
                inventory.AddKey(keyID);
                Destroy(gameObject);
            }
        }
    }
}
