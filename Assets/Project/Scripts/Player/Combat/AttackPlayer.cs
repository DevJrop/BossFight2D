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

        private void Update()
        {
            HandleInput();
        }
        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            GameObject bulletObject = objectPool.GetObject(attack.bulletPrefab);

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