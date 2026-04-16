using System.Collections;
using Project.Scripts.Enemy.Core;
using Project.Scripts.Player.Controller;
using UnityEngine;
namespace Project.Scripts.Player.Combat
{
    public class Bullet : MonoBehaviour
    {
        private ObjectPool pool;
        private float lifeTime;

        private BulletOwner owner;
        private float damage;

        private Rigidbody2D rb;
        private TrailRenderer trail;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            trail = GetComponent<TrailRenderer>();
        }
        public void SetPool(ObjectPool pool, float lifeTime, BulletOwner owner, float damage)
        {
            this.pool = pool;
            this.lifeTime = lifeTime;
            this.owner = owner;
            this.damage = damage;

            StopAllCoroutines();
            StartCoroutine(LifeRoutine());
        }
        IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(lifeTime);
            ReturnToPool();
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Wall"))
            {
                ReturnToPool();
                return;
            }
            if (owner == BulletOwner.Player && other.CompareTag("Enemy"))
            {
                ApplyDamage(other);
            }
            else if (owner == BulletOwner.Enemy && other.CompareTag("Player"))
            {
                ApplyDamage(other);
            }
        }
        private void ApplyDamage(Collider2D other)
        {
            Health health = other.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }

            ReturnToPool();
        }
        private void ReturnToPool()
        {
            rb.linearVelocity = Vector2.zero;
            StopAllCoroutines();

            if (trail != null)
            {
                trail.Clear();
            }
            pool.ReturnObject(gameObject);
        }
    }
}