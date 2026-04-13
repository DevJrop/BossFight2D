using Unity.VisualScripting;
using UnityEngine;

namespace Project.Scripts.Enemy.Combat
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]
    public class Attack : ScriptableObject
    {
        public float damage;
        public GameObject bulletPrefab;
        public float cooldown;
        public float speed;


    }
}
