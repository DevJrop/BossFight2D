using System;
using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float maxMana = 5f;
    [SerializeField] private float drainSpeed = 1f;
    [SerializeField] private float regenValue = 0.5f; // ✅ Cantidad por tick
    [SerializeField] private float regenTime = 0.1f;   // ✅ Tiempo entre ticks

    private float currentMana;
    private bool isActive;
    private Coroutine rechargeCoroutine;
    
    public event Action<bool> OnPowerUpStateChanged;
    public event Action<float> OnManaChanged;
    
    public bool IsActive => isActive;

    private void Start()
    {
        currentMana = maxMana;
        OnManaChanged?.Invoke(GetManaNormalized());
        StartRecharge();
    }

    private void Update()
    {
        HandleInput();
        
        if (isActive)
        {
            HandleManaConsumption();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentMana > 0 && !isActive)
        {
            ActivatePowerUp();
        }
    }
    
    private void HandleManaConsumption()
    {
        currentMana -= drainSpeed * Time.deltaTime;

        if (currentMana <= 0)
        {
            currentMana = 0;
            DeactivatePowerUp();
        }
        
        OnManaChanged?.Invoke(GetManaNormalized());
    }
    
    private void ActivatePowerUp()
    {
        isActive = true;
        OnPowerUpStateChanged?.Invoke(true);
        StopRecharge();
    }
    
    private void DeactivatePowerUp()
    {
        isActive = false;
        OnPowerUpStateChanged?.Invoke(false);
        StartRecharge();
    }
    
    // ✅ CORRUTINA IDÉNTICA A TU EJEMPLO
    private IEnumerator RegenMana()
    {
        while (!isActive && currentMana < maxMana)
        {
            if (currentMana < maxMana)
            {
                currentMana += regenValue;
                currentMana = Mathf.Min(currentMana, maxMana);
                OnManaChanged?.Invoke(GetManaNormalized());
                yield return new WaitForSeconds(regenTime);
            }
            else
            {
                yield return null;
            }
        }
    }
    
    private void StartRecharge()
    {
        if (rechargeCoroutine != null)
        {
            StopCoroutine(rechargeCoroutine);
        }
        rechargeCoroutine = StartCoroutine(RegenMana());
    }
    
    private void StopRecharge()
    {
        if (rechargeCoroutine != null)
        {
            StopCoroutine(rechargeCoroutine);
            rechargeCoroutine = null;
        }
    }
    
    public float GetManaNormalized()
    {
        return currentMana / maxMana;
    }
    
    public bool TryConsumeMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            OnManaChanged?.Invoke(GetManaNormalized());
            return true;
        }
        return false;
    }
    
    private void OnDestroy()
    {
        StopRecharge();
    }
}