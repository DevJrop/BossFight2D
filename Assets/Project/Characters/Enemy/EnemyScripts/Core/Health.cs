using System;
using System.Collections;
using Project.Characters.Enemy.EnemyScripts.Movement;
using Project.Characters.Player.PlayerScripts.Controller;
using Project.Characters.Player.PlayerScripts.Movement;
using Project.Scripts.Controller;
using Unity.Cinemachine;
using UnityEngine;

namespace Project.Characters.Enemy.EnemyScripts.Core
{
    public class Health : MonoBehaviour, IDamageable
    {
        private float currentHealth;
        [SerializeField]private float maxHealth = 100;
        [SerializeField] private float flashTime;
        private CinemachineBasicMultiChannelPerlin noise;
        [SerializeField] private CinemachineCamera virtualCamera;
        [SerializeField] private float amplitude;
        [SerializeField] private float frequency;
        private bool isAlive;
        public event Action<float, float> OnHealthChanged;
        [SerializeField] private DeathEvent deathEvent;
        [SerializeField] private AudioClip damageSound;
        private PlayerSoundController playerSoundController;
        [SerializeField][Range(0,0.5f)] private float volume;
        [SerializeField] private EnemyMove enemyMove;
        
        private void Awake()
        {
            playerSoundController = GetComponent<PlayerSoundController>();
            currentHealth = maxHealth;
            noise = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            isAlive = true;
        }
        public void TakeDamage(float damage)
        {
            if (enemyMove != null && enemyMove.IsUnderGround) return;
            if (!isAlive) return;
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            
            NotifyHealthBar();
            StartCoroutine(ApplyEffectDamage());
            if (currentHealth <= 0)
            {
                isAlive = false;
                deathEvent.Die();
            }
        }
        private void NotifyHealthBar()
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
        IEnumerator ApplyEffectDamage()
        {
            playerSoundController.PlayDamage(damageSound, volume);
            SpriteRenderer colorFlash = GetComponent<SpriteRenderer>(); 
            colorFlash.color = Color.red;
            noise.AmplitudeGain = amplitude;
            noise.FrequencyGain = frequency;
            yield return new WaitForSeconds(flashTime);
            noise.AmplitudeGain = 0f;
            noise.FrequencyGain = 0f;
            colorFlash.color = Color.white;
        }
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
    }
}
