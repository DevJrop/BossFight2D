using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace Project.Scripts.Enemy.Core
{
    public class Health : MonoBehaviour, IDamageable
    {
        private float currentHealth;
        [SerializeField]private float maxHealth = 100;
        public event Action<float, float> OnHealthChanged;

        private void Awake()
        {
            currentHealth = maxHealth;
        }
        public void TakeDamage(float damage)
        {
            if (currentHealth <= 0) return;
            
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            
            NotifyHealthBar();

        }
        private void NotifyHealthBar()
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
    }
}
