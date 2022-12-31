using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles {
    public class VerdantGas : ModProjectile {
        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 3;

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.alpha = 40;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source) {
            Projectile.frame = Main.rand.Next(Main.projFrames[Type]);

            base.OnSpawn(source);
        }

        public override void AI() {
            // Fade out
            Projectile.alpha += 5;
            if (Projectile.alpha >= 255) {
                Projectile.Kill();
            }

            // Scale up
            Projectile.scale += 0.08f;

            // Velocity slowdown
            Projectile.velocity *= 0.95f;

            // Animate
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 12) {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Type]) {
                    Projectile.frame = 0;
                }
            }

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = 20;
            height = 20;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}
