using UnityEngine;

public class Door : MonoBehaviour
{
    public string requiredKeyID = "Level1Key";
    public GameObject doorObject;
    public float openSpeed = 2f;
    private bool isOpening = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeyInventory inventory = other.GetComponent<KeyInventory>();
            if (inventory != null && inventory.HasKey(requiredKeyID))
            {
                isOpening = true;
            }
            else
            {
                Debug.Log("Kapı için gerekli anahtar yok: " + requiredKeyID);
            }
        }
    }

    void Update()
    {
        if (isOpening && doorObject != null)
        {
            // Kapıyı yukarı doğru kaydır (istersen rotasyonla da açabilirsin)
            doorObject.transform.position += Vector3.up * openSpeed * Time.deltaTime;
        }
    }
}