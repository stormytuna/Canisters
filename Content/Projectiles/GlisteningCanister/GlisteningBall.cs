using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.GlisteningCanister
{
    /// <summary>
    ///     Splitting balls that the depleted glistening canister fires
    /// </summary>
    public class GlisteningBall : ModProjectile
    {
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        private bool IsParent => Projectile.ai[0] == 0f;

        public override void AI() {
            // Dust
            for (int i = 0; i < 3; i++) {
                if (Main.rand.NextBool()) {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch, Alpha: Main.rand.Next(100, 200), Scale: Main.rand.NextFloat(1f, 1.2f));
                    d.velocity *= 0.3f;
                    d.noGravity = true;
                    d.noLight = true;
                }
                if (Main.rand.NextBool()) {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, Alpha: Main.rand.Next(100, 200), Scale: Main.rand.NextFloat(1f, 1.2f));
                    d.velocity *= 0.3f;
                    d.noGravity = true;
                    d.noLight = true;
                }
            }

            // Lighting
            Lighting.AddLight(Projectile.Center, TorchID.Ichor);

            base.AI();
        }

        public override void Kill(int timeLeft) {
            // Mini dust explosion
            for (int i = 0; i < 10; i++) {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(1f, 1.2f));
                dust.velocity = velocity;
                dust.noGravity = true;
            }

            // Create our two children
            if (IsParent && Main.myPlayer == Projectile.owner) {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f);

                Vector2 offset = velocity * -10f;
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + offset, velocity, Type, Projectile.damage / 3, 0f, Projectile.owner, 1f);
                proj.timeLeft = 10;
                proj.extraUpdates = 1;
                proj.tileCollide = false;

                proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center - offset, -velocity, Type, Projectile.damage / 3, 0f, Projectile.owner, 1f);
                proj.timeLeft = 10;
                proj.extraUpdates = 1;
                proj.tileCollide = false;
            }

            base.Kill(timeLeft);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(BuffID.Ichor, 600);

            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
