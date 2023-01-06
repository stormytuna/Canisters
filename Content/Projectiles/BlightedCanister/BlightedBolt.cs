using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.BlightedCanister
{
    /// <summary>
    ///     More of a helper projectile, it enables our lightning bolt to have pierce
    /// </summary>
    public class BlightedBolt : ModProjectile
    {
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 200;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        private Vector2 StartLocation => new Vector2(Projectile.ai[0], Projectile.ai[1]);

        private bool firstFrame = true;

        public override void AI() {
            if (firstFrame) {
                Projectile.ai[0] = Projectile.Center.X;
                Projectile.ai[1] = Projectile.Center.Y;
                Projectile.netUpdate = true;

                firstFrame = false;
            }

            base.AI();
        }

        public override bool? CanCutTiles() => false;

        public override void Kill(int timeLeft) {
            // Lightning dust
            LightningHelper.MakeDust(StartLocation, Projectile.Center, DustID.CursedTorch, 1.2f, 500f, 2f);

            // Mini dust explosion
            for (int i = 0; i < 10; i++) {
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
