
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
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
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 0;

            base.SetStaticDefaults();
        }
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 16;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 90;

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

        public override void Kill(int timeLeft) {
            for (int i = 0; i < 20; i++) {
                // Our base dust properties
                Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(1f, 1.3f));
                dust.velocity = velocity;
                dust.noGravity = true;

                if (Main.rand.NextBool(3)) {
                    // 1/3 dust becomes medium dust
                    float sizeMult = Main.rand.NextFloat(1.3f, 1.6f);
                    dust.scale *= sizeMult;
                    dust.velocity /= sizeMult;
                } else if (Main.rand.NextBool(4)) {
                    // 1/4 of the rest become little grass that's gravity effected
                    dust.velocity.X /= 4f;
                    dust.velocity.Y = MathF.Abs(dust.velocity.Y) / -4f;
                    dust.noGravity = false;
                }
            }

            base.Kill(timeLeft);
        }

        private Asset<Texture2D> _texture;
        new private Asset<Texture2D> Texture { get => _texture ??= ModContent.Request<Texture2D>(base.Texture); }

        public override bool PreDraw(ref Color lightColor) {
            // Draw our afterimages
            for (int i = 1; i < Projectile.oldPos.Length; i += 2) {
                Vector2 position = Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.height / 2);
                Rectangle sourceRect = new(0, 0, Texture.Width(), Texture.Height());
                Color color = Projectile.GetAlpha(lightColor) * (((float)Projectile.oldPos.Length - (float)i) / (float)Projectile.oldPos.Length);
                Vector2 origin = Texture.Size() / 2f;
                Main.EntitySpriteDraw(Texture.Value, position, sourceRect, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }

            return base.PreDraw(ref lightColor);
        }
    }
}
