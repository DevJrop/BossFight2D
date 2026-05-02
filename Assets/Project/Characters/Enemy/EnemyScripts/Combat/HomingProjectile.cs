using UnityEngine;

namespace Project.Characters.Enemy.EnemyScripts.Combat
{
    public class HomingProjectile : AttackExecutorBase
    {
        private void ShootAtPlayer(AttackContext ctx)
        {
            if (ctx.player == null) return;
            
            Vector3 currentPlayerPosition = ctx.player.position;
            Vector2 direction = (currentPlayerPosition - ctx.firePoint.position).normalized;
            
            if (ctx.animator != null)
                ctx.animator.SetTrigger("Attack");
        }

        public override void Execute(AttackContext ctx)
        {
            ShootAtPlayer(ctx);
        }
    }
}
