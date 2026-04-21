using System;
using UnityEngine;

namespace Project.Scripts.Controller
{
    public class DeathEvent : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] componentsToDisable;
        [SerializeField] private Rigidbody2D rb2DsToDisable;
        [SerializeField] private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb2DsToDisable = GetComponent<Rigidbody2D>();
        }

        public void Die()
        {
            foreach (var comp in componentsToDisable)
            {
                comp.enabled = false;
            }
            rb2DsToDisable.linearVelocity =  Vector2.zero;
            animator.SetTrigger("Die");
        }

    }
}
