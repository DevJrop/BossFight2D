using System;
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
        [SerializeField]private Attack attack;

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
            pool.ReturnObject(gameObject);
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            DestroyAtWall(other);

            GetDamage(other);
        }

        private void GetDamage(Collider2D other)
        {
            Health health = other.GetComponent<Health>();

            if (other.GetComponent<Health>())
            {
                health.TakeDamage(attack.damage);
                Destroy(gameObject);
            }
        }
        private void DestroyAtWall(Collider2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
