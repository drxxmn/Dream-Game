using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int Id;
    public int Amount = 1;
    public AudioClip PickupClip;

    private GameObject _UISFX;

    void Start()
    {
        if (PickupClip != null) _UISFX = GameObject.Find("UI").transform.Find("SFX").gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<PlayerInventory>().AddItem(Id, Amount);
            if (PickupClip != null && _UISFX != null)
            {
                _UISFX.GetComponent<AudioSource>().PlayOneShot(PickupClip);
            }
            Destroy(gameObject);
        }
    }
}
