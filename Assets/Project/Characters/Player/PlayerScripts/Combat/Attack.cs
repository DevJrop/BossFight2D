using UnityEngine;

namespace Project.Characters.Player.PlayerScripts.Combat
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]
    [System.Serializable]
    public class Attack : ScriptableObject
    {
        public float damage;
        public GameObject bulletPrefab;
        public float cooldown;
        public float speed;
        public float timeAfterDestroy;
    }
}
