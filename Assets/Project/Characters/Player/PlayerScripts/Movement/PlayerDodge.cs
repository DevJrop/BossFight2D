using System;
using System.Collections;
using Project.Characters.Player.PlayerScripts.Controller;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Characters.Player.PlayerScripts.Movement
{
    public class PlayerDodge : MonoBehaviour
    {
        [Header("Dodge Movement")]
        [SerializeField]private float dashForce;
        [SerializeField]private float dashTime;
        [SerializeField]private float dodgeCost;
        private bool isDashing;
        private bool isInvulnerable;
        
        [Header("Stamina")]
        [SerializeField]private float currentStamina;
        [SerializeField]private float maxStamina;
        [SerializeField]private Image staminaBar;
        [SerializeField]private float regenTime;
        [SerializeField] private float regenValue;
        public event Action<float, float> OnStaminaChanged;
        
        [Header("References")]
        private PlayerMove playerMove;
        private Rigidbody2D rb;
        [SerializeField]private TrailRenderer trail;
        
        [Header("Camera Shake")]
        [SerializeField] private CinemachineCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin noise;
        private PlayerSoundController playerSoundController;
        [SerializeField][Range(0,0.5f)] private float volume;
        
        void Start()
        {
            playerSoundController = GetComponent<PlayerSoundController>();
            noise = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            playerMove = GetComponent<PlayerMove>();
            rb = GetComponent<Rigidbody2D>();
            trail.enabled = false;
            currentStamina = maxStamina;
        }
        void Update()
        {
            DodgeInput();
            StartCoroutine(RegenStamina());
        }
        private void DodgeInput()
        {
            if (UIManager.instance.IsPaused) return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerSoundController.PlayDodge(volume);
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
            isInvulnerable = true;
            isDashing = true;
            trail.enabled = true;
            noise.AmplitudeGain = 1.2f;
            noise.FrequencyGain = 2f;
            rb.linearVelocity = playerMove.MoveInput * dashForce;
            yield return new WaitForSeconds(dashTime);
            noise.AmplitudeGain = 0f;
            noise.FrequencyGain = 0f;
            trail.enabled = false;
            isDashing = false;
            isInvulnerable = false;
        }

        IEnumerator RegenStamina()
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += regenValue * Time.deltaTime;
                OnStaminaChanged?.Invoke(currentStamina, maxStamina);
                yield return new WaitForSeconds(regenTime);
            }
        }
        public bool IsInvulnerable => isInvulnerable;
        public bool IsDashing => isDashing;
        public float MaxStamina => maxStamina;
        public float CurrentStamina => currentStamina;
    }
}
