using UnityEngine;

namespace Project.Characters.Enemy.EnemyScripts.Combat
{
    [System.Serializable]
    public class AttackData : ScriptableObject
    {
        public GameObject bulletPrefab;
        public float damage;
        public float cooldown;
        public float speed;
        
    }
}