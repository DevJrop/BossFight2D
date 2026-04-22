using System;
using System.Collections;
using UnityEngine;

namespace Project.Characters.Player.PlayerScripts.Combat
{
    public class PowerUp : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private float maxMana = 5f;
        [SerializeField] private float drainSpeed = 1f;
        [SerializeField] private float regenValue = 0.5f;
        [SerializeField] private float regenTime = 0.1f;

        private float currentMana;
        private bool isActive;
        
        public event Action<bool> OnPowerUpStateChanged;
        public event Action<float> OnManaChanged;

        public bool IsActive => isActive;

        void Start()
        {
            currentMana = maxMana;
            isActive = false;
            OnManaChanged?.Invoke(GetManaNormalized());
        }

        void Update()
        {
            HandleInput();

            if (isActive)
                HandleManaConsumption();
            
            // Iniciar la coroutine de regeneración constantemente (como en PlayerDodge)
            StartCoroutine(RegenMana());
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.E) && currentMana >= maxMana && !isActive)
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
        }

        private void DeactivatePowerUp()
        {
            isActive = false;
            OnPowerUpStateChanged?.Invoke(false);
        }

        private IEnumerator RegenMana()
        {
            // Solo regenerar si NO está activo y la mana NO está llena
            if (!isActive && currentMana < maxMana)
            {
                currentMana += regenValue * Time.deltaTime;
                currentMana = Mathf.Min(currentMana, maxMana);
                
                OnManaChanged?.Invoke(GetManaNormalized());
                
                yield return new WaitForSeconds(regenTime);
            }
        }

        public float GetManaNormalized()
        {
            return currentMana / maxMana;
        }
    }
}