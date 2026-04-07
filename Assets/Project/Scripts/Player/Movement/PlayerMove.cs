using System.Collections;
using UnityEngine;
namespace Project.Scripts.Player.Movement
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;

        private Rigidbody2D rb;
        private Animator anim;

        private Vector2 moveInput;
        private Vector2 lastMove;
        private bool isDashing;

        [SerializeField]private float dashForce;
        [SerializeField]private float dashTime;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            isDashing = !isDashing;
        }
        private void Update()
        {
            InputMovement();
            UpdateAnimations();
            HandleFlip();
            Dodge();
        }
        private void FixedUpdate()
        {
            if (!isDashing) return;
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
            if (moveInput.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1f, 1f);
            }
        }
        private void Dodge()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isDashing = false;
                StartCoroutine(Dash());
            }
        }
        IEnumerator Dash()
        {
            rb.linearVelocity = moveInput * dashForce;
            yield return new WaitForSeconds(dashTime);
            isDashing = true;
        }
    }
}