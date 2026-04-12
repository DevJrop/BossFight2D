using UnityEngine;
namespace Project.Scripts.Player.Movement
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        private Rigidbody2D rb;
        private Animator anim;
        public Vector2 moveInput;
        private Vector2 lastMove;
        PlayerDodge playerDodge;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            playerDodge = GetComponent<PlayerDodge>();
        }
        private void Update()
        {
            InputMovement();
            UpdateAnimations();
            HandleFlip();
        }
        private void FixedUpdate()
        {
            if(!playerDodge.isDashing) return;
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
        private void UpdateAnimations()
        {
            anim.SetFloat("Horizontal", lastMove.x);
            anim.SetFloat("Vertical", lastMove.y);
            anim.SetFloat("Speed", moveInput.magnitude);
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
            return;
        }
    }
}