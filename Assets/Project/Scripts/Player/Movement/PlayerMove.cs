using UnityEngine;

namespace Project.Scripts.Player.Movement
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;

        private Rigidbody2D rb;
        private Animator animation;

        private Vector2 moveInput;
        private Vector2 lastMove;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animation = GetComponent<Animator>();
        }

        private void Update()
        {
            InputMovement();
            UpdateAnimations();
            HandleFlip();
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
            animation.SetFloat("Horizontal", lastMove.x);
            animation.SetFloat("Vertical", lastMove.y);
            
            animation.SetFloat("Speed", moveInput.magnitude);
        }

        private void HandleFlip()
        {
            if (moveInput.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1f, 1f);
            }
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = moveInput * speed;
        }
    }
}