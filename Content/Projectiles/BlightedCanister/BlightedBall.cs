
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.BlightedCanister
{
    /// <summary>
    ///     Balls that the blighted canister releases when it explodes
    /// </summary>
    public class BlightedBall : ModProjectile
    {
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 16;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 50;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        public override void AI() {
            // Dust
            if (Main.rand.NextBool()) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, Scale: Main.rand.NextFloat(1f, 1.2f));
                d.noGravity = true;
                d.noLight = true;
            }

            // Rotate velocity so it spins around
            Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0] * 0.07f);

            // Point where it's travelling
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            base.AI();
        }

        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = 8;
            height = 8;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(BuffID.CursedInferno, 600);

            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
