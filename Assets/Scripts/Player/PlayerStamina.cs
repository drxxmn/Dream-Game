using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Tooltip("Maximum amount of stamina for the player")]
    public float MaxStamina = 1f;

    [Tooltip("Time in seconds before stamina starts to regenerate")]
    public float RegenerationDelay = 1f;

    [Tooltip("Time in seconds before stamina starts to regenerate")]
    public float RegenerationRate = 1f;

    public float CurStamina { get; private set; }

    private float _lastUsedTime = -1;

    private void Start()
    {
        CurStamina = MaxStamina;
    }

    private void Update()
    {
        if (_lastUsedTime >= 0 && Time.time - _lastUsedTime >= RegenerationDelay)
        {
            RegenerateStamina();
        }
    }

    private void RegenerateStamina()
    {
        float extraStamina = RegenerationRate * Time.deltaTime;
        AddStamina(extraStamina);
    }

    public void AddStamina(float amount)
    {
        CurStamina = Mathf.Clamp(CurStamina + amount, 0, MaxStamina);

        if(CurStamina == MaxStamina)
         {
            _lastUsedTime = -1;
         }
    }

    public void ReduceStamine(float amount)
    {
        CurStamina -= amount;
        _lastUsedTime = Time.time;
    }
}
