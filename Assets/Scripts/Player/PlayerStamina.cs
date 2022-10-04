using UnityEngine;
using StarterAssets;

public class PlayerStamina : MonoBehaviour
{
    [Tooltip("Maximum amount of stamina for the player")]
    public float MaxStamina = 1f;

    [Tooltip("Rate at which stamina regenerates")]
    public float RegenerationRate = 2f;

    public float CurStamina { get; private set; }

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
        AddStamina(extraStamina);
    }

    public void AddStamina(float amount)
    {
        CurStamina = Mathf.Clamp(CurStamina + amount, 0, MaxStamina);
    }

    public void ReduceStamina(float amount)
    {
        CurStamina -= amount;
    }
}
