using Project.Characters.Enemy.EnemyScripts.Core;
using Project.Characters.Player.PlayerScripts.Combat;
using Project.Characters.Player.PlayerScripts.Movement;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Controller
{
    public class UpdateBar : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private Health health;
        [SerializeField] private Image healthBar;

        [Header("PowerUp")]
        [SerializeField] private PowerUp powerUp;
        [SerializeField] private Image fillImage;

        [Header("Stamina")]
        [SerializeField] private PlayerDodge dodge;
        [SerializeField] private Image fillStamina;

        [Header("Reload")]
        [SerializeField] private AttackPlayer reload;
        [SerializeField] private Image fillReload;

        private void OnEnable()
        {
            if (health != null)
            {
                health.OnHealthChanged += UpdateHealthBar;
            }
            if (powerUp != null)
            {
                powerUp.OnManaChanged += UpdateManaBar;
            }
            if (dodge != null)
            {
                dodge.OnStaminaChanged += UpdateStaminaBar;
            }
            if (reload != null)
            {
                reload.OnReloadChange += UpdateReloadBar;
            }
        }
        private void OnDisable()
        {
            if (health != null)
            {
                health.OnHealthChanged -= UpdateHealthBar;
            }
            if (powerUp != null)
            {
                powerUp.OnManaChanged -= UpdateManaBar;
            }
            if (dodge != null)
            {
                dodge.OnStaminaChanged -= UpdateStaminaBar;
            }
            if (reload != null)
            {
                reload.OnReloadChange -= UpdateReloadBar;
            }
        }
        private void Start()
        {
            if (health != null)
            {
                UpdateHealthBar(health.CurrentHealth, health.MaxHealth);
            }
            if (dodge != null)
            {
                UpdateStaminaBar(dodge.CurrentStamina, dodge.MaxStamina);
            }
            if (powerUp != null)
            {
                UpdateManaBar(powerUp.GetManaNormalized());
            }
            if (fillReload != null)
            {
                UpdateReloadBar(1f, 1f);
            }
        }
        private void UpdateHealthBar(float current, float max)
        {
            if (healthBar == null) return;
            healthBar.fillAmount = current / max;
        }
        private void UpdateManaBar(float normalizedMana)
        {
            if (fillImage == null) return;
            fillImage.fillAmount = normalizedMana;
        }
        private void UpdateStaminaBar(float current, float max)
        {
            if (fillStamina == null) return;
            fillStamina.fillAmount = current / max;
        }
        private void UpdateReloadBar(float current, float max)
        {
            if (fillReload == null) return;
            fillReload.fillAmount = current / max;
        }
    }
}