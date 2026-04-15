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
        [SerializeField] private Attack attack;

        private Rigidbody2D rb;
        private TrailRenderer trail;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            trail = GetComponent<TrailRenderer>();
        }

        public void SetPool(ObjectPool pool, float lifeTime)
        {
            this.pool = pool;
            this.lifeTime = lifeTime;
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
            if (other.CompareTag("Enemy"))
            {
                Health health = other.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(attack.damage);
                    ReturnToPool();
                }
            }
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