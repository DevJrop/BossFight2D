using UnityEngine;

namespace Project.Characters.Enemy.EnemyScripts.Combat
{
    public abstract class AttackExecutorBase : MonoBehaviour
    {
        public abstract void  Execute(AttackContext context);
    }
}
