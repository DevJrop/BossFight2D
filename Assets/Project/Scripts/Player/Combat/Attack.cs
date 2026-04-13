using UnityEngine;

namespace Project.Scripts.Player.Combat
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]
    public class Attack : ScriptableObject
    {
        public float damage;
        public GameObject bulletPrefab;
        public float cooldown;
        public float speed;
        public float timeAfterDestroy;
    }
}
