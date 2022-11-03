using UnityEngine;

public class SoulPickup : MonoBehaviour
{
    [Tooltip("Amount of points towards a new stamina point")]
    public int ProgressPoints = 1;
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
            FindObjectOfType<PlayerStamina>().IncreaseProgress(ProgressPoints);
            if (PickupClip != null && _UISFX != null)
            {
                _UISFX.GetComponent<AudioSource>().PlayOneShot(PickupClip);
            }
            Destroy(gameObject);
        }
    }
}
