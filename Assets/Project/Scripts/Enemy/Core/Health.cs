using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace Project.Scripts.Enemy.Core
{
    public class Health : MonoBehaviour, IDamageable
    {
        //TODO: get and set, no public methods
        
        public float currentHealth = 100;
        public float maxHealth = 100;
        public event Action<float, float> OnHealthChanged;

        private void Start()
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
    }
}
