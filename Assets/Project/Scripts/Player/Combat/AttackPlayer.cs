using Project.Scripts.Player.Controller;
using UnityEngine;
namespace Project.Scripts.Player.Combat
{
    public class AttackPlayer : MonoBehaviour
    {
        [SerializeField] private Attack attack;
        private Bullet bullet;
        [SerializeField] private Transform firePoint;
        public bool canFire;
        [SerializeField] private ObjectPool objectPool;
        BulletOwner bulletOwner;
        private void Start()
        {
            canFire = !canFire;
        }
        private void Update()
        {
            CanAttack();
        }
        private void CanAttack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                canFire = true;
                ThrowBullet();
            }
            else
            {
                canFire = false;
            }
        }
        private void ThrowBullet()
        {
            GameObject bulletObject = objectPool.GetObject();
            
            bulletObject.transform.position = firePoint.position;
            bulletObject.transform.rotation = firePoint.rotation;
            
            Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            
            rb.AddForce(firePoint.right * attack.speed, ForceMode2D.Impulse);
            
            Bullet bulletScript = bulletObject.GetComponent<Bullet>();

            bulletScript.SetPool(objectPool, attack.timeAfterDestroy, BulletOwner.Player, attack.damage);
        }
    }
}
