using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.StarfallCanister
{
    /// <summary>
    ///     Pixie dust and fallen star canister
    /// </summary>
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
}