using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Player.Movement
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] private PlayerDodge dodge;
        [SerializeField] private Image fill;

        private void OnEnable()
        {
            dodge.OnStaminaChanged += UpdateBar;
        }

        private void OnDisable()
        {
            dodge.OnStaminaChanged -= UpdateBar;
        }

        private void Start()
        {
            UpdateBar(dodge.CurrentStamina, dodge.MaxStamina);
        }

        private void UpdateBar(float current, float max)
        {
            fill.fillAmount = current / max;
        }
    }
}