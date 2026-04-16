using Project.Scripts.Player.Combat;
using Project.Scripts.Player.Controller;
using UnityEngine;

namespace Project.Scripts.Enemy.Combat
{
    public class EnemyAttack : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] private ObjectPool pool;

        [Header("Pattern Settings")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private int bulletCount = 12;
        [SerializeField] private float fireRate = 1f;

        [Header("Bullet Settings")]
        [SerializeField] private float bulletSpeed = 5f;
        [SerializeField] private float bulletLifeTime = 3f;
        [SerializeField] private float damage = 10f;

        private float timer;

        private void Update()
        {
            HandleShooting();
        }
        private void HandleShooting()
        {
            timer += Time.deltaTime;

            if (timer >= fireRate)
            {
                ExecuteRadialPattern();
                timer = 0;
            }
        }
        private void ExecuteRadialPattern()
        {
            float angleStep = 360f / bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Vector2 direction = GetDirectionFromAngle(angle);

                SpawnBullet(direction);
            }
        }
        private void SpawnBullet(Vector2 direction)
        {
            GameObject obj = pool.GetObject();

            obj.transform.position = firePoint.position;
            obj.transform.rotation = Quaternion.identity;

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * bulletSpeed;

            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.SetPool(pool, bulletLifeTime, BulletOwner.Enemy, damage);
        }
        private Vector2 GetDirectionFromAngle(float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }
}