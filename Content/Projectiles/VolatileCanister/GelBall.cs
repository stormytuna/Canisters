using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.VolatileCanister
{
    /// <summary>
    ///     The gel ball fired from a depleted volatile canister
    /// </summary>
    public class GelBall : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Gel Stream");

            base.SetStaticDefaults();
        }

        public override void OnSpawn(IEntitySource source) {
            Projectile.rotation = Main.rand.NextRadian();

            base.OnSpawn(source);
        }

        public override void SetDefaults() {
            // Base stats
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 1;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        /// <summary>
        /// 0f == Unaffected by gravity <br/>
        /// 1f == Affected by gravity
        /// </summary>
        private ref float AI_State => ref Projectile.ai[0];

        private ref float AI_FrameCounter => ref Projectile.ai[1];

        public override void AI() {
            // Increment our frame counter
            AI_FrameCounter++;

            // Make dust
            for (int i = 0; i < 3; i++) {
                // Our base dust properties
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Water, Scale: Main.rand.NextFloat(1f, 1.2f));
                dust.velocity *= 0.2f;
            }

            // State 0 => Unaffected by gravity
            if (AI_State == 0f) {
                if (AI_FrameCounter >= 20f) {
                    AI_State = 1f;
                }

                return;
            }

            // State 1 => Affected by gravity
            Projectile.velocity += Vector2.UnitY * 0.2f;

            base.AI();
        }

        public override void Kill(int timeLeft) {
            // Little dust explosion
            for (int i = 0; i < 20; i++) {
                // Our base dust properties
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Water, Scale: Main.rand.NextFloat(1f, 1.2f));
                dust.velocity *= Main.rand.NextVector2Circular(15f, 15f);
                dust.noGravity = true;
            }

            // Tiny sound
            var soundStyle = SoundID.SplashWeak with {
                MaxInstances = 0,
                Volume = 0.2f,
                PitchRange = (0.9f, 1f)
            };
            SoundEngine.PlaySound(soundStyle, Projectile.Center);

            base.Kill(timeLeft);
        }
    }
}
