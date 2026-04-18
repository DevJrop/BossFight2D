using System.Collections;
using Project.Scripts.Enemy.Core;
using Project.Scripts.Player.Controller;
using Project.Scripts.Player.Movement;
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
        private GameObject prefab;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            trail = GetComponent<TrailRenderer>();
        }
        public void SetPool(ObjectPool pool, GameObject prefab, float lifeTime, BulletOwner owner, float damage)
        {
            this.pool = pool;
            this.lifeTime = lifeTime;
            this.owner = owner;
            this.damage = damage;
            this.prefab = prefab;
            
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

            if (other.CompareTag("Player") &&other.GetComponent<PlayerDodge>().IsInvulnerable) return;
            if (owner == BulletOwner.Enemy && other.CompareTag("Player"))
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
            pool.ReturnObject(gameObject, prefab);
        }
    }
}