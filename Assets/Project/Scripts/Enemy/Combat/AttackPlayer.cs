using UnityEngine;
namespace Project.Scripts.Enemy.Combat
{
    public class AttackPlayer : MonoBehaviour
    {
        [SerializeField] private Attack attack;
        private Bullet bullet;
        private bool isAttacking;
        [SerializeField] private Transform firePoint;
        
        private void Update()
        {
            CanAttack();
        }

        private void CanAttack()
        {
            if (Input.GetMouseButton(0))
            {
                ThrowBullet();
            }
        }

        private void ThrowBullet()
        {
            GameObject bulletObject = Instantiate(attack.bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rbBullet = bulletObject.GetComponent<Rigidbody2D>();
            Bullet bulletScript = bulletObject.GetComponent<Bullet>();
            rbBullet.AddForce(firePoint.right * attack.speed, ForceMode2D.Impulse);
        }
        
    }
}
