
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Enemy.Core
{
    public class UpdateBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Image healthBar;

        private void OnEnable()
        {
            if (health != null)
            {
                health.OnHealthChanged += ChangeBar;
            }
        }
        private void OnDisable()
        {
            if (health != null)
            {
                health.OnHealthChanged -= ChangeBar;
            }
        }
        private void Start()
        {
            if (health != null)
            {
                ChangeBar(health.CurrentHealth, health.MaxHealth);
            }
        }
        private void ChangeBar(float current, float max)
        {
            float ratio = current / max;
            healthBar.fillAmount = ratio;
        }
    }
}
