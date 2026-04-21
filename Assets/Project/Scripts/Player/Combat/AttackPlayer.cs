using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Player.Controller;
using UnityEngine;

namespace Project.Scripts.Player.Combat
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
        
        private Attack currentAttack;
        
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
            if (Input.GetMouseButton(0))
            {
                if (chargerCapacity <= counterShoots)
                {
                    StartCoroutine(Reload());
                }
                Shoot();
            }
        }
        IEnumerator Reload()
        {
            isReloading = true;
            yield return new WaitForSeconds(chargerTime);
            counterShoots = 0;
            isReloading = false;
        }
        
        private void Shoot()
        {
            Transform target = FindClosestEnemy();
            
            GameObject bulletObject = objectPool.GetObject(currentAttack.bulletPrefab);
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
        
        private Transform FindClosestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            Transform closest = null;
            float minDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = enemy.transform;
                }
            }
            return closest;
        }
    }
}