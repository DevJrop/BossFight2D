using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
namespace Project.Scripts.Player.Movement
{
    public class PlayerDodge : MonoBehaviour
    {
        [SerializeField]private float dashForce;
        [SerializeField]private float dashTime;
        public bool isDashing;
        private PlayerMove playerMove;
        private Rigidbody2D rb;
        [SerializeField]private TrailRenderer trail;
        [SerializeField] private CinemachineCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin noise;
        void Start()
        {
            noise = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            playerMove = GetComponent<PlayerMove>();
            rb = GetComponent<Rigidbody2D>();
            trail.enabled = false;
        }
        void Update()
        {
            DodgeInput();
        }
        private void DodgeInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                StartCoroutine(Dash());
            }
        }
        IEnumerator Dash()
        {
            isDashing = true;
            trail.enabled = true;
            noise.AmplitudeGain = 1.2f;
            noise.FrequencyGain = 2f;
            rb.linearVelocity = playerMove.moveInput * dashForce;
            yield return new WaitForSeconds(dashTime);
            noise.AmplitudeGain = 0f;
            noise.FrequencyGain = 0f;
            trail.enabled = false;
            isDashing = false;
        }
    }
}
