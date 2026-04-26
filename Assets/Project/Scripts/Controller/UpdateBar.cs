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
        [SerializeField] private Image fill;

        private void OnEnable()
        {
            if (health != null)
            {
                health.OnHealthChanged += UpdateHealthBar;
            }
            if (powerUp != null)
            {
                powerUp.OnManaChanged += UpdateManaBar;
                
                UpdateManaBar(powerUp.GetManaNormalized());
            }
            if (dodge != null)
            {
                dodge.OnStaminaChanged += UpdateStaminaBar;
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
        }
        private void UpdateHealthBar(float current, float max)
        {
            float ratio = current / max;
            healthBar.fillAmount = ratio;
        }
        private void UpdateManaBar(float normalizedMana)
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = normalizedMana;
            }
        }
        private void UpdateStaminaBar(float current, float max)
        {
            fill.fillAmount = current / max;
        }
    }
}
