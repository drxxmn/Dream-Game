using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int Id;
    public int Amount = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<PlayerInventory>().AddItem(Id, Amount);
            Destroy(gameObject);
        }
    }
}
