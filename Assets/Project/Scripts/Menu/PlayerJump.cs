using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Menu
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float jumpDuration = 0.5f;

        private void Start()
        {
            Jump();
        }

        public void Jump()
        {
            player.transform.DOMoveY(
                    player.transform.position.y + jumpHeight,
                    jumpDuration
                )
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}