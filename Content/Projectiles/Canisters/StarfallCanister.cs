using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.Canisters
{
    public class StarfallCanister : ModProjectile
    {
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = 2;
            Projectile.alpha = 100;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        public override string Texture => "Canisters/Content/Items/Canisters/StarfallCanister";

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if (Projectile.alpha != 255) {
                Explode();
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (Projectile.alpha != 255) {
                Explode();
                return false;
            }

            return base.OnTileCollide(oldVelocity);
        }

        private void Explode() {
            SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

            // Rain 3-5 stars where this canister explodes
            Vector2 startPosition = Projectile.Center - new Vector2(Main.rand.NextFloat(-300, 300), 1200f);
            int numStars = Main.rand.Next(3, 6);
            for (int i = 0; i < numStars; i++) {
                // Create the actual star
                Vector2 position = startPosition + Main.rand.NextVector2Circular(300f, 25f) + new Vector2(0f, i * -100f);
                Vector2 velocity = position.DirectionTo(Projectile.Center) * 7f;
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<FallingStar>(), Projectile.damage / numStars, Projectile.knockBack / 2f, Projectile.owner);
                proj.rotation = Main.rand.NextRadian();

                // Little dust explosion
                for (int j = 0; j < 10; j++) {
                    Vector2 dustPosition = Main.rand.NextVector2Square(-10f, 10f);
                    Vector2 dustVelocity = Main.rand.NextVector2Circular(5f, 5f);
                    Dust d = Dust.NewDustPerfect(dustPosition, DustID.YellowStarDust);
                    d.velocity = dustVelocity;
                }
            }

            Projectile.TurnToExplosion(96, 96);
        }
    }

    public class StarfallCanister_Depleted : ModProjectile
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
            Projectile.rotation += CanisterHelpers.EaseIn(0.1f, 0.4f, AI_FrameCount / 40f, 3);

            // Split into smart firing stars after 90 frames
            if (AI_FrameCount >= 40) {
                IEnumerable<NPC> targets = CanisterHelpers.FindNearbyNPCs(100f * 16f, Projectile.Center);
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