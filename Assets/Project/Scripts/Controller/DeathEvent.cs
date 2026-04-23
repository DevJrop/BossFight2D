using System;
using System.Collections;
using Project.Characters.Player.PlayerScripts.Controller;
using UnityEngine;

namespace Project.Scripts.Controller
{
    public class DeathEvent : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] componentsToDisable;
        [SerializeField] private Rigidbody2D rb2DsToDisable;
        [SerializeField] private Animator animator;
        [SerializeField][Range(0,0.5f)] private float volumeLose;
        private PlayerSoundController playerSoundController;
        private void Awake()
        {
            playerSoundController = GetComponent<PlayerSoundController>();
            animator = GetComponent<Animator>();
            rb2DsToDisable = GetComponent<Rigidbody2D>();
        }

        public void Die()
        {
            playerSoundController.PlayLose(volumeLose);
            foreach (var comp in componentsToDisable)
            {
                comp.enabled = false;
            }
            rb2DsToDisable.linearVelocity =  Vector2.zero;
            animator.SetTrigger("Die");
            StartCoroutine(OpenUI());
        }

        IEnumerator OpenUI()
        {
            yield return new WaitForSeconds(2);
            UIManager.instance.YouDieManager();
        }
        

    }
}
