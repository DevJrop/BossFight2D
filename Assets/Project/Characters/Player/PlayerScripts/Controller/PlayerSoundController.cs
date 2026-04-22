using UnityEngine;

namespace Project.Characters.Player.PlayerScripts.Controller
{
    public class PlayerSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip fire;
        [SerializeField] private AudioClip dodge;
        [SerializeField] private AudioClip damage;
        [SerializeField] private AudioClip walk;
        [SerializeField] private AudioClip reload;
        
        public void PlayFire()
        {
            audioSource.PlayOneShot(fire);
        }

        public void PlayDodge()
        {
            audioSource.PlayOneShot(dodge);
        }

        public void PlayDamage()
        {
            audioSource.PlayOneShot(damage);
        }
        
        public void PlayWalk(AudioClip clip)
        {
            audioSource.PlayOneShot(walk);
        }
        public void PlayReload()
        {
            audioSource.PlayOneShot(reload);
        }
    }
}
