using System;
using System.Collections;
using Project.Characters.Player.PlayerScripts.Controller;
using Project.Characters.Player.PlayerScripts.Core;
using Project.Scripts.Controller;
using UnityEngine;
namespace Project.Characters.Player.PlayerScripts.Combat
{
    public class AttackPlayer : MonoBehaviour
    {
        #region Attack Configuration

        [Header("Attack Data")]
        [SerializeField] private Attack attack;
        [SerializeField] private Attack powerUpAttack;

        [Header("Attack References")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private PowerUp powerUpHoming;

        private Attack currentAttack;

        #endregion
        #region Reload System

        [Header("Reload Settings")]
        [SerializeField] private float chargerCapacity = 6f;
        [SerializeField] private float chargerTime = 2f;

        private int counterShoots;
        private bool isReloading;

        public event Action<float, float> OnReloadChange;
        public float ChargerTime => chargerTime;

        #endregion
        #region Fire Rate System

        [Header("Fire Rate")]
        [SerializeField] private float fireRate = 0.2f;

        private float fireTimer;

        #endregion
        #region Auto Shoot System

        [Header("Auto Shoot")]
        [SerializeField] private float autoShootRate = 0.2f;

        private float autoShootTimer;

        #endregion
        #region Audio System

        [Header("Audio Settings")]
        [SerializeField] [Range(0f, 0.5f)] private float volumeShoot;
        [SerializeField] [Range(0f, 0.5f)] private float volumeReload;

        private PlayerSoundController playerSoundController;

        #endregion
        private void Awake()
        {
            playerSoundController = GetComponent<PlayerSoundController>();
        }
        private void Start()
        {
            currentAttack = attack;
            if (powerUpHoming != null)
            {
                powerUpHoming.OnPowerUpStateChanged += OnPowerUpStateChanged;
            }
            OnReloadChange?.Invoke(1f, 1f);
        }
        private void OnDestroy()
        {
            if (powerUpHoming != null)
            {
                powerUpHoming.OnPowerUpStateChanged -= OnPowerUpStateChanged;
            }
        }
        private void Update()
        {
            if (fireTimer > 0)
            {
                fireTimer -= Time.deltaTime;
            }
            if (powerUpHoming != null && powerUpHoming.IsActive)
            {
                AutoShoot();
            }
            else
            {
                HandleInputShoot();
            }
        }
        private void OnPowerUpStateChanged(bool isActive)
        {
            currentAttack = isActive && powerUpAttack != null
                ? powerUpAttack
                : attack;
        }
        private void HandleInputShoot()
        {
            if (UIManager.instance.IsPaused) return;
            if (isReloading) return;
            if (fireTimer > 0) return;
            if (Input.GetMouseButton(0))
            {
                if (counterShoots >= chargerCapacity)
                {
                    StartCoroutine(Reload());
                    return;
                }
                Shoot();
                fireTimer = fireRate;
            }
        }
        private void AutoShoot()
        {
            if (isReloading) return;
            autoShootTimer += Time.deltaTime;
            if (autoShootTimer >= autoShootRate)
            {
                autoShootTimer = 0f;
                if (counterShoots >= chargerCapacity)
                {
                    StartCoroutine(Reload());
                    return;
                }
                Shoot();
            }
        }
        private IEnumerator Reload()
        {
            isReloading = true;
            playerSoundController.PlayReload(volumeReload);
            float elapsed = 0f;
            OnReloadChange?.Invoke(0f, 1f);
            while (elapsed < chargerTime)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / chargerTime;
                OnReloadChange?.Invoke(progress, 1f);
                yield return null;
            }
            counterShoots = 0;
            isReloading = false;
            OnReloadChange?.Invoke(1f, 1f);
        }
        private void Shoot()
        {
            Transform target = FindEnemy();
            GameObject bulletObject = objectPool.GetObject(currentAttack.bulletPrefab);
            playerSoundController.PlayFire(volumeShoot);
            counterShoots++;
            bulletObject.transform.position = firePoint.position;
            bulletObject.transform.rotation = firePoint.rotation;
            Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = firePoint.right * currentAttack.speed;
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.SetPool(
                objectPool,
                currentAttack.bulletPrefab,
                currentAttack.timeAfterDestroy,
                BulletOwner.Player,
                currentAttack.damage
            );
            bool isHoming = powerUpHoming != null && powerUpHoming.IsActive;
            bullet.SetTarget(target, isHoming);
        }
        private Transform FindEnemy()
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            return enemy != null ? enemy.transform : null;
        }
    }
}