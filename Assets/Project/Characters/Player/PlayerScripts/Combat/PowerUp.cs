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
        private Coroutine rechargeCoroutine;
        public event Action<bool> OnPowerUpStateChanged;
        public event Action<float> OnManaChanged;

        public bool IsActive => isActive;

        private void OnEnable()
        {
            ResetState();
        }

        private void Update()
        {
            HandleInput();

            if (isActive)
                HandleManaConsumption();
        }
        private void ResetState()
        {
            currentMana = maxMana;
            isActive = false;

            OnManaChanged?.Invoke(GetManaNormalized());

            StartRecharge();
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
            StopRecharge();
        }

        private void DeactivatePowerUp()
        {
            isActive = false;
            OnPowerUpStateChanged?.Invoke(false);
            StartRecharge();
        }

        private IEnumerator RegenRoutine()
        {
            while (!isActive && currentMana < maxMana)
            {
                currentMana += regenValue * Time.deltaTime;
                currentMana = Mathf.Min(currentMana, maxMana);

                OnManaChanged?.Invoke(GetManaNormalized());

                yield return new WaitForSeconds(regenTime);
            }

            rechargeCoroutine = null;
        }

        private void StartRecharge()
        {
            if (rechargeCoroutine != null)
                StopCoroutine(rechargeCoroutine);

            rechargeCoroutine = StartCoroutine(RegenRoutine());
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
    }
}