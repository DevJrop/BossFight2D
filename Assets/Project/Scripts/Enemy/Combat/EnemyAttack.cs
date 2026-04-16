using Project.Scripts.Enemy.Movement;
using Project.Scripts.Player.Combat;
using Project.Scripts.Player.Controller;
using UnityEngine;

namespace Project.Scripts.Enemy.Combat
{
    public class EnemyAttack : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] private ObjectPool pool;

        [Header("References")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform player;
        [SerializeField] private EnemyMove move;

        [Header("Animation")]
        [SerializeField] private Animator animator;

        [Header("Radial Pattern")]
        [SerializeField] private int bulletCount = 12;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float rotationSpeed = 50f;
        [SerializeField] private Attack radialAttack;

        [Header("Target Attack")]
        [SerializeField] private float targetFireRate = 2f;
        [SerializeField] private Attack targetAttack;

        [Header("Wall Attack")]
        [SerializeField] private int wallBulletCount = 8;
        [SerializeField] private float wallFireRate = 0.4f;
        [SerializeField] private float wallArcAngle = 60f;
        [SerializeField] private float wallRotationSpeed = 120f;
        [SerializeField] private Attack wallAttack;

        private enum AttackPhase
        {
            Radial,
            Target,
            Wall
        }

        private AttackPhase currentPhase;

        private float timer;
        private float targetTimer;
        private float wallTimer;
        private float rotationOffset;

        private bool wasWaitingLastFrame;

        private void Update()
        {
            if (!move.IsWaiting)
            {
                wasWaitingLastFrame = false;
                return;
            }

            if (!wasWaitingLastFrame && move.IsWaiting)
            {
                SelectNewPhase();
                ResetTimers();

                if (animator != null)
                    animator.SetTrigger("Idle");
            }

            wasWaitingLastFrame = true;

            ExecuteCurrentPhase();
        }

        private void SelectNewPhase()
        {
            int random = Random.Range(0, 3);
            currentPhase = (AttackPhase)random;
        }
        private void ResetTimers()
        {
            timer = 0;
            targetTimer = 0;
            wallTimer = 0;
        }
        private void ExecuteCurrentPhase()
        {
            switch (currentPhase)
            {
                case AttackPhase.Radial:
                    HandleRadialPattern();
                    break;

                case AttackPhase.Target:
                    HandleTargetAttack();
                    break;

                case AttackPhase.Wall:
                    HandleWallAttack();
                    break;
            }
        }
        private void HandleRadialPattern()
        {
            rotationOffset += rotationSpeed * Time.deltaTime;

            timer += Time.deltaTime;

            if (timer >= fireRate)
            {
                ExecuteRotatingRadial();

                if (animator != null)
                    animator.SetTrigger("Attack");

                timer = 0;
            }
        }
        private void ExecuteRotatingRadial()
        {
            float angleStep = 360f / bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep + rotationOffset;
                Vector2 direction = GetDirectionFromAngle(angle);

                SpawnBullet(direction, radialAttack);
            }
        }
        private void HandleTargetAttack()
        {
            targetTimer += Time.deltaTime;

            if (targetTimer >= targetFireRate)
            {
                ShootAtPlayer();
                targetTimer = 0;
            }
        }
        private void ShootAtPlayer()
        {
            if (player == null) return;
            
            Vector3 currentPlayerPosition = player.position;
            Vector2 direction = (currentPlayerPosition - firePoint.position).normalized;

            SpawnBullet(direction, targetAttack);

            if (animator != null)
                animator.SetTrigger("Attack");
        }
        private void HandleWallAttack()
        {
            rotationOffset += wallRotationSpeed * Time.deltaTime;

            wallTimer += Time.deltaTime;

            if (wallTimer >= wallFireRate)
            {
                if (animator != null)

                    ExecuteWallAttack();

                if (animator != null)
                    animator.SetTrigger("Attack");

                wallTimer = 0;
            }
        }
        private void ExecuteWallAttack()
        {
            float startAngle = rotationOffset - wallArcAngle / 2f;
            float angleStep = wallArcAngle / (wallBulletCount - 1);

            for (int i = 0; i < wallBulletCount; i++)
            {
                float angle = startAngle + (angleStep * i);
                Vector2 direction = GetDirectionFromAngle(angle);

                SpawnBullet(direction, wallAttack);
            }
        }
        private void SpawnBullet(Vector2 direction, Attack attackData)
        {
            GameObject obj = pool.GetObject(attackData.bulletPrefab);

            obj.transform.position = firePoint.position;
            obj.transform.rotation = Quaternion.identity;

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * attackData.speed;

            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.SetPool(
                pool,
                attackData.bulletPrefab,
                attackData.timeAfterDestroy,
                BulletOwner.Enemy,
                attackData.damage
            );
        }

        private Vector2 GetDirectionFromAngle(float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }
}