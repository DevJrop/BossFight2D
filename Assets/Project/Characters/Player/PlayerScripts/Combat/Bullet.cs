using System.Collections;
using Project.Characters.Enemy.EnemyScripts.Core;
using Project.Characters.Player.PlayerScripts.Controller;
using Project.Characters.Player.PlayerScripts.Core;
using Project.Characters.Player.PlayerScripts.Movement;
using UnityEngine;

namespace Project.Characters.Player.PlayerScripts.Combat
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

        [SerializeField] private GameObject explosionPrefab;

        private Transform target;
        private bool isHoming;
        [SerializeField] private float homingSpeed = 10f; 

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
        
        //Esto lo hace el enemigo
        public void SetTarget(Transform target, bool isHoming)
        {
            this.target = target;
            this.isHoming = isHoming;
        }
        //Esto lo hace el lifetimer
        IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(lifeTime);
            ReturnToPool();
        }
        
        private void Update()
        {
            if (!isHoming || target == null) return;
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * homingSpeed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        //Bullet collision
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
            if (other.CompareTag("Player") && other.GetComponent<PlayerDodge>().IsInvulnerable) return;
            if (owner == BulletOwner.Enemy && other.CompareTag("Player"))
            {
                ApplyDamage(other);
            }
        }
        //Bullet
        private void ApplyDamage(Collider2D other)
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            ReturnToPool();
        }
        //Pooling
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