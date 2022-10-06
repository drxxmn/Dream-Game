using UnityEngine;
using StarterAssets;

public class PlayerStamina : MonoBehaviour
{
    [Tooltip("Maximum amount of stamina for the player")]
    public float MaxStamina = 1f;

    [Tooltip("Rate at which stamina regenerates")]
    public float RegenerationRate = 2f;

    [Tooltip("How many shards needed to get a full stamina point")]
    public int ShardsToUpgrade = 4;

    public float CurStamina { get; private set; }
    public int UpgradeProgress { get; private set; } = 0;

    private ThirdPersonController _thirdPersonController;

    private void Start()
    {
        CurStamina = MaxStamina;
        _thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if (CurStamina < MaxStamina && _thirdPersonController.Grounded)
        {
            RegenerateStamina();
        }
    }

    private void RegenerateStamina()
    {
        float extraStamina = RegenerationRate * Time.deltaTime;
        IncreaseCurStamina(extraStamina);
    }

    private void IncreaseCurStamina(float amount)
    {
        CurStamina = Mathf.Clamp(CurStamina + amount, 0, MaxStamina);
    }

    public void ReduceCurStamina(float amount)
    {
        CurStamina -= amount;
    }

    public void IncreaseProgress(int amount)
    {
        UpgradeProgress += amount;

        if (UpgradeProgress >= ShardsToUpgrade)
        {
            int pointsToUpgrade = UpgradeProgress / ShardsToUpgrade;
            int remainder = UpgradeProgress % ShardsToUpgrade;
            MaxStamina += pointsToUpgrade;
            UpgradeProgress = remainder;
        }
    }
}
