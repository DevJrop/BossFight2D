using UnityEngine;

namespace Project.Characters.Player.PlayerScripts.Controller
{
    public class PlayerSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip fire;
        [SerializeField] private AudioClip dodge;
        [SerializeField] private AudioClip walk;
        [SerializeField] private AudioClip reload;
        [SerializeField] private AudioClip damage;
        
        public void PlayFire(float volume)
        {
            audioSource.PlayOneShot(fire, volume);
        }

        public void PlayDodge()
        {
            audioSource.PlayOneShot(dodge);
        }

        public void PlayDamage(AudioClip clip, float volume)
        {
            audioSource.PlayOneShot(damage, volume);
        }
        
        public void PlayWalk(AudioClip clip, float volume)
        {
            audioSource.PlayOneShot(walk, volume);
        }
        public void PlayReload(float volume)
        {
            audioSource.PlayOneShot(reload, volume);
        }
    }
}
