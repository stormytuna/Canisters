using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.BlightedCanister
{
    /// <summary>
    ///     Cursed flame canister
    /// </summary>
    public class BlightedCanister : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Blighted Canister");

            base.SetStaticDefaults();
        }

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

        public override string Texture => "Canisters/Content/Items/Canisters/BlightedCanister";

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

            // TODO: Create spiraly cursed flame balls
            float rotationOffset = Main.rand.NextRadian();
            float sign = MathF.Sign(Main.rand.NextFloat(-1f, 1f));
            for (int i = 0; i < 5; i++) {
                float rot = (((float)i / 5f) * MathHelper.TwoPi) + rotationOffset;
                Vector2 velocity = rot.ToRotationVector2() * 3f;
                Vector2 positionOffset = velocity * 2f;
                Vector2 position = Projectile.Center + positionOffset;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<BlightedBall>(), Projectile.damage / 3, Projectile.knockBack / 3f, Projectile.owner, sign);
            }

            // More dust 
            for (int i = 0; i < 90; i++) {
                // Our base dust properties
                Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(0.8f, 1.2f));
                dust.velocity = velocity;
                dust.noGravity = true;

                if (Main.rand.NextBool(3)) {
                    // 1/3 dust becomes medium dust
                    float sizeMult = Main.rand.NextFloat(1f, 1.5f);
                    dust.scale *= sizeMult;
                    dust.velocity /= sizeMult;
                } else if (Main.rand.NextBool(4)) {
                    // 1/4 of the rest become little grass that's gravity effected
                    dust.velocity.X /= 4f;
                    dust.velocity.Y = MathF.Abs(dust.velocity.Y) / -4f;
                    dust.noGravity = false;
                }
            }

            Projectile.TurnToExplosion(96, 96);
        }
    }
}