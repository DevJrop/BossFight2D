using System.Collections;
using Project.Scripts.Player.Controller;
using UnityEngine;

namespace Project.Scripts.Player.Combat
{
    public class Bullet : MonoBehaviour
    {
        private ObjectPool pool;
        private float lifeTime;
        
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
            if (other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
