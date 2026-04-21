using System.Collections;
using UnityEngine;

namespace Project.Characters.Enemy.EnemyScripts.Movement
{
    public class EnemyMove : MonoBehaviour
    {
        [Header("Path")]
        [SerializeField] private Transform[] spots;

        [Header("Movement Settings")]
        [SerializeField] private float timeSpot = 1f;
        [SerializeField] private float pointClose = 0.2f;
        [SerializeField] private float speedBetweenPoints = 3f;

        private Transform enemy;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private int currentSpot;
        private int randPos;

        private bool isWaiting; 
        private bool isMoving;  

        private void Start()
        {
            enemy = transform;
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (spots.Length == 0) return;

            if (isMoving)
            {
                enemy.position = Vector2.MoveTowards(
                    enemy.position,
                    spots[currentSpot].position,
                    Time.deltaTime * speedBetweenPoints
                );

                if (Vector2.Distance(enemy.position, spots[currentSpot].position) < pointClose)
                {
                    FinishMovement();
                }

                return;
            }

            if (isWaiting) return;

            enemy.position = Vector2.MoveTowards(
                enemy.position,
                spots[currentSpot].position,
                Time.deltaTime * speedBetweenPoints
            );

            CheckArrival();
        }

        private void CheckArrival()
        {
            if (Vector2.Distance(enemy.position, spots[currentSpot].position) < pointClose && !isWaiting)
            {
                isWaiting = true;
                StartCoroutine(WaitAndMove());
            }
        }

        private IEnumerator WaitAndMove()
        {
            yield return new WaitForSeconds(timeSpot);

            if (animator != null)
                animator.SetTrigger("Hide");
            yield return new WaitForSeconds(1);
            
            if (spriteRenderer != null)
                spriteRenderer.enabled = false;
            do
            {
                randPos = Random.Range(0, spots.Length);
            }
            while (randPos == currentSpot);
            currentSpot = randPos;
            isMoving = true;
        }
        private void FinishMovement()
        {
            isMoving = false;

            if (spriteRenderer != null)
                spriteRenderer.enabled = true;

            if (animator != null)
                animator.SetTrigger("Exit");

            isWaiting = false;
        }
        public bool IsWaiting => isWaiting && !isMoving;
    }
}