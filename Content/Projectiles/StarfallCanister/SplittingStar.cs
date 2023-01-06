using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.StarfallCanister
{
    /// <summary>
    ///     Splitting star that the depleted starfall canister uses
    /// </summary>
    public class SplittingStar : ModProjectile
    {
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        private ref float AI_FrameCount => ref Projectile.ai[0];

        private bool firstFrame = false;

        public override void AI() {
            // First frame
            if (firstFrame) {
                firstFrame = false;
                Projectile.rotation = Main.rand.NextRadian();
                Projectile.velocity *= Main.rand.NextFloat(0.9f, 1.1f);
            }

            // Slow down in the air
            Projectile.velocity *= 0.93f;

            // Spin in the air
            Projectile.rotation += Helpers.EaseIn(0.1f, 0.4f, AI_FrameCount / 40f, 3);

            // Split into smart firing stars after 90 frames
            if (AI_FrameCount >= 40) {
                IEnumerable<NPC> targets = Helpers.FindNearbyNPCs(100f * 16f, Projectile.Center);
                for (int i = 0; i < 5; i++) {
                    NPC target = Main.rand.Next(targets.ToArray());
                    Vector2 velocity = Projectile.DirectionTo(target.Center) * 10f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<StarShard>(), Projectile.damage / 5, Projectile.knockBack / 3f, Projectile.owner);
                }
                Projectile.Kill();
            }

            AI_FrameCount++;

            base.AI();
        }
    }
}
