using UnityEngine;
using StarterAssets;
using UnityEngine.Events;
using Yarn.Unity;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private VariableStorageBehaviour _variableStorage;
    
    [Tooltip("Maximum amount of stamina for the player")]
    public float MaxStamina = 1f;

    [Tooltip("Rate at which stamina regenerates")]
    public float RegenerationRate = 2f;

    [Tooltip("How many shards needed to get a full stamina point")]
    public int ShardsToUpgrade = 4;

    public float CurStamina { get; private set; }
    public int UpgradeProgress { get; private set; } = 0;

    [System.Serializable]
    private class Events
    {
        public UnityEvent ProgressIncreased = new UnityEvent();
        public UnityEvent MaxStaminaIncreased = new UnityEvent();
        public UnityEvent StaminaEmpty = new UnityEvent();
        public UnityEvent StaminaFull = new UnityEvent();
    }
    [SerializeField] private Events _events;

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
        CurStamina = Mathf.Clamp(CurStamina - amount, 0, MaxStamina);

        if (CurStamina == 0)
        {
            _events.StaminaEmpty.Invoke();
        }
    }

    [YarnCommand("increase_stamina")]
    public void IncreaseProgress(int amount)
    {
        UpgradeProgress += amount;

        if (UpgradeProgress >= ShardsToUpgrade)
        {
            int pointsToUpgrade = UpgradeProgress / ShardsToUpgrade;
            int remainder = UpgradeProgress % ShardsToUpgrade;
            MaxStamina += pointsToUpgrade;
            UpgradeProgress = remainder;
            _events.MaxStaminaIncreased.Invoke();
        }
        
        _events.ProgressIncreased.Invoke();
    }
}
