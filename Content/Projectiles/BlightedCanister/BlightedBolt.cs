using Canisters.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.BlightedCanister
{
    /// <summary>
    ///     More of a helper projectile, it lets our lightning bolt have pierce
    /// </summary>
    public class BlightedBolt : ModProjectile
    {
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 200;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        private Vector2 startLocation;
        private bool firstFrame = true;

        public override void AI() {
            if (firstFrame) {
                startLocation = Projectile.Center;
                firstFrame = false;
            }

            base.AI();
        }

        public override bool? CanCutTiles() => false;

        public override void Kill(int timeLeft) {
            // Lightning dust
            LightningSystem.MakeDust(startLocation, Projectile.Center, DustID.CursedTorch, 1.4f, 80f, 1f);

            // Mini dust explosion
            for (int i = 0; i < 13; i++) {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(1f, 1.2f));
                dust.velocity = velocity;
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(BuffID.CursedInferno, 600);

            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
