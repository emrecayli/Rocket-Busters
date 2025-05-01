using UnityEngine;
using System.Collections.Generic;

public class KeyInventory : MonoBehaviour
{
    private HashSet<string> keys = new HashSet<string>();

    public void AddKey(string keyID)
    {
        keys.Add(keyID);
        Debug.Log("Anahtar alındı: " + keyID);
    }

    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
    }
}