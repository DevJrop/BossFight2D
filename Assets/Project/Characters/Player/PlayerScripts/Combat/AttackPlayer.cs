using System;
using System.Collections;
using Project.Characters.Player.PlayerScripts.Controller;
using Project.Characters.Player.PlayerScripts.Core;
using UnityEngine;

namespace Project.Characters.Player.PlayerScripts.Combat
{
    public class AttackPlayer : MonoBehaviour
    {
        [Header("Attack Data")]
        [SerializeField] private Attack attack;
        [SerializeField] private Attack powerUpAttack;

        [Header("References")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private float chargerCapacity;
        [SerializeField] private float chargerTime;
        private int counterShoots;
        private bool isReloading;
        [SerializeField] private PowerUp powerUpHoming;
        private float autoShootTimer;
        [SerializeField] private float autoShootRate = 0.2f;
        
        [Header("Fire Rate")]
        [SerializeField] private float fireRate = 0.2f;
        private float fireTimer;
        
        private Attack currentAttack;
        
        private PlayerSoundController playerSoundController;
        [SerializeField][Range(0,0.5f)] private float volumeShoot;
        [SerializeField][Range(0,0.5f)] private float volumeReload;

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
        }
        private void OnDestroy()
        {
            if (powerUpHoming != null)
            {
                powerUpHoming.OnPowerUpStateChanged -= OnPowerUpStateChanged;
            }
        }
        private void OnPowerUpStateChanged(bool isActive)
        {
            if (isActive)
            {
                currentAttack = powerUpAttack != null ? powerUpAttack : attack;
            }
            else
            {
                currentAttack = attack;
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

        private void AutoShoot()
        {
            autoShootTimer += Time.deltaTime;

            if (autoShootTimer >= autoShootRate)
            {
                autoShootTimer = 0f;
                Shoot();
            }
        }
        private void HandleInputShoot()
        {
            if (isReloading) return;
            if (fireTimer > 0) return;
            
            if (Input.GetMouseButton(0))
            {
                if (chargerCapacity <= counterShoots)
                {
                    StartCoroutine(Reload());
                }
                Shoot();
                fireTimer = fireRate;
            }
        }
        IEnumerator Reload()
        {
            playerSoundController.PlayReload(volumeReload);
            isReloading = true;
            yield return new WaitForSeconds(chargerTime);
            counterShoots = 0;
            isReloading = false;
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
            
            if (enemy != null)
            {
                return enemy.transform;
            }
            
            return null;
        }
    }
}