using Project.Characters.Player.PlayerScripts.Controller;
using UnityEngine;

namespace Project.Characters.Player.PlayerScripts.Movement
{
    public class PlayerMove : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 8f;

        [Header("Footsteps")]
        [SerializeField] private float walkStepRate = 0.5f;
        [SerializeField] private float runStepRate = 0.3f;
        [SerializeField] private AudioClip walkStep;
        [SerializeField] private AudioClip runStep;

        private float stepTimer;

        private PlayerSoundController playerSoundController;

        private float currentSpeed;
        private Rigidbody2D rb;
        private Animator anim;

        private Vector2 moveInput;
        private Vector2 lastMove;

        PlayerDodge playerDodge;
        private bool isMoving;
        
        [SerializeField][Range(0,0.5f)] private float volume;

        private void Awake()
        {
            playerSoundController = GetComponent<PlayerSoundController>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            playerDodge = GetComponent<PlayerDodge>();

            isMoving = true;
        }

        private void Update()
        {
            if (!isMoving) return;

            InputMovement();
            HandleSpeed();
            UpdateAnimations();
            HandleFlip();
            HandleFootsteps(); 
        }

        private void FixedUpdate()
        {
            if (playerDodge.IsDashing) return;

            rb.linearVelocity = moveInput * speed;
        }

        private void InputMovement()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            moveInput = new Vector2(moveX, moveY).normalized;

            if (moveInput != Vector2.zero)
            {
                lastMove = moveInput;
            }
        }

        private void HandleSpeed()
        {
            if (moveInput != Vector2.zero)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 1f, acceleration * Time.deltaTime);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
            }
        }
        
        private void HandleFootsteps()
        {
            if (moveInput == Vector2.zero) return;

            stepTimer -= Time.deltaTime;

            bool isRunning = currentSpeed > 0.6f;

            float stepRate = isRunning ? runStepRate : walkStepRate;

            if (stepTimer <= 0f)
            {
                AudioClip clip = isRunning ? runStep : walkStep;

                playerSoundController.PlayWalk(clip, volume);

                stepTimer = stepRate;
            }
        }

        private void UpdateAnimations()
        {
            anim.SetFloat("Horizontal", lastMove.x);
            anim.SetFloat("Vertical", lastMove.y);
            anim.SetFloat("Speed", currentSpeed);
        }

        private void HandleFlip()
        {
            SpriteRenderer spritePlayer = GetComponent<SpriteRenderer>();

            if (moveInput.x < 0)
            {
                spritePlayer.flipX = true;
                lastMove.x = -lastMove.x;
            }
            else if (moveInput.x > 0)
            {
                spritePlayer.flipX = false;
            }
        }

        public Vector2 MoveInput => moveInput;
    }
}