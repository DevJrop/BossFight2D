using UnityEngine;

using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float maxMana = 5f;
    [SerializeField] private float drainSpeed = 1f; // consumo por segundo
    [SerializeField] private float rechargeSpeed = 0.5f;

    private float currentMana;
    private bool isActive;

    public bool IsActive => isActive;

    private void Start()
    {
        currentMana = maxMana;
    }

    private void Update()
    {
        HandleInput();
        HandleMana();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentMana > 0 && !isActive)
        {
            isActive = true;
        }
    }

    private void HandleMana()
    {
        if (isActive)
        {
            currentMana -= drainSpeed * Time.deltaTime;

            if (currentMana <= 0)
            {
                currentMana = 0;
                isActive = false;
            }
        }
        else
        {
            // 🔋 recarga automática (puedes quitar esto si no quieres)
            if (currentMana < maxMana)
            {
                currentMana += rechargeSpeed * Time.deltaTime;
            }
        }
    }

    public float GetManaNormalized()
    {
        return currentMana / maxMana;
    }
}