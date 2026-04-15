using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

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
        public event Action<float, float> OnStaminaChanged;
        [SerializeField]private float currentStamina;
        [SerializeField]private float maxStamina;
        [SerializeField]private float dodgeCost;
        [SerializeField]private Image staminaBar;
        void Start()
        {
            noise = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            playerMove = GetComponent<PlayerMove>();
            rb = GetComponent<Rigidbody2D>();
            trail.enabled = false;
            currentStamina = maxStamina;
        }
        void Update()
        {
            DodgeInput();
        }
        private void DodgeInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryDodge();
            }
        }

        private void TryDodge()
        {
            if (currentStamina < dodgeCost)
            {
                return;
            }
            currentStamina -= dodgeCost;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            StartCoroutine(Dash());
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
        
        public float MaxStamina => maxStamina;

        public float CurrentStamina => currentStamina;
    }
}
