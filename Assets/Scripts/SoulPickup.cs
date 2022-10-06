using UnityEngine;

public class SoulPickup : MonoBehaviour
{
    [Tooltip("Amount of points towards a new stamina point")]
    public int ProgressPoints = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<PlayerStamina>().IncreaseProgress(ProgressPoints);
            Destroy(gameObject);
        }
    }
}
