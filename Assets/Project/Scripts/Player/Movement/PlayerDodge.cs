using System.Collections;
using UnityEngine;
namespace Project.Scripts.Player.Movement
{
    public class PlayerDodge : MonoBehaviour
    {
        [SerializeField]private float dashForce;
        [SerializeField]private float dashTime;
        public bool isDashing;
        PlayerMove playerMove;
        private Rigidbody2D rb;
        SpriteRenderer sr;
        void Start()
        {
            playerMove = GetComponent<PlayerMove>();
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            isDashing = !isDashing;
        }
        void Update()
        {
            DodgeInput();
        }
        private void DodgeInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isDashing = false;
                StartCoroutine(Dash());
            }
        }
        IEnumerator Dash()
        {
            rb.linearVelocity = playerMove.moveInput * dashForce;
            yield return new WaitForSeconds(dashTime);
            isDashing = true;
        }
    }
}
