using System.Collections;
using Project.Scripts.Player.Controller;
using UnityEngine;

namespace Project.Scripts.Player.Combat
{
    public class AttackPlayer : MonoBehaviour
    {
        [Header("Attack Data")]
        [SerializeField] private Attack attack;

        [Header("References")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private float chargerCapacity;
        [SerializeField] private float chargerTime;
        private int counterShoots;
        private bool isReloading;
        private void Update()
        {
            HandleInputShoot();
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
            GameObject bulletObject = objectPool.GetObject(attack.bulletPrefab);
            counterShoots++;

            bulletObject.transform.position = firePoint.position;
            bulletObject.transform.rotation = firePoint.rotation;

            Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = firePoint.right * attack.speed;

            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.SetPool(
                objectPool,
                attack.bulletPrefab,
                attack.timeAfterDestroy,
                BulletOwner.Player,
                attack.damage
            );
        }
    }
}