
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles
{
    public class FallingStar : ModProjectile
    {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
            Projectile.alpha = 255;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        public override void AI() {
            // See if we can collide with tiles now
            if (!Projectile.tileCollide) {
                if (!Collision.SolidTiles(Projectile.position, Projectile.width, Projectile.height)) {
                    Projectile.tileCollide = true;
                }
            }

            // Fade in
            Projectile.alpha -= 20;

            // Spinny
            Projectile.rotation += 0.2f;

            // Dust
            if (Main.rand.NextBool(3)) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.YellowStarDust, Scale: Main.rand.NextFloat(0.6f, 0.9f));
                d.noGravity = true;
            }

            // Lighting
            Lighting.AddLight(Projectile.Center, 1f, 1f, 153f / 255f);

            base.AI();
        }

        public override void Kill(int timeLeft) {
            // Dust explosion
            for (int i = 0; i < 20; i++) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.YellowStarDust, Scale: Main.rand.NextFloat(0.6f, 0.9f));
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(15f, 15f);
            }

            base.Kill(timeLeft);
        }

        private Asset<Texture2D> _star;
        private Asset<Texture2D> Star {
            get {
                _star ??= ModContent.Request<Texture2D>("Canisters/Content/Projectiles/FallingStar");
                return _star;
            }
        }

        private Asset<Texture2D> _trail;
        private Asset<Texture2D> Trail {
            get {
                _trail ??= ModContent.Request<Texture2D>("Canisters/Content/Projectiles/FallingStar_Trail");
                return _trail;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            // Trail
            Vector2 trailPos = Projectile.Center - Main.screenPosition;
            Rectangle trailSourceRect = new(0, 0, Trail.Width(), Trail.Height());
            float trailRotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            trailPos -= Projectile.velocity.SafeNormalize(Vector2.Zero) * 30f;
            Color trailColor = new(255, 215, 0);
            Vector2 trailOrigin = Trail.Size() / 2f;
            Main.EntitySpriteDraw(Trail.Value, trailPos, trailSourceRect, trailColor, trailRotation, trailOrigin, Projectile.scale, SpriteEffects.None, 0);

            Rectangle starSourceRect = new(0, 0, Star.Width(), Star.Height());
            Color starColor = new(255, 230, 205);
            Vector2 starOrigin = Star.Size() / 2f;

            // Afterimages
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++) {
                Vector2 afterImagePos = Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.height / 2);
                Color afterImageColor = starColor * (((float)Projectile.oldPos.Length - (float)i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(Star.Value, afterImagePos, starSourceRect, afterImageColor, Projectile.rotation, starOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            // Star
            Vector2 starPos = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(Star.Value, starPos, starSourceRect, starColor, Projectile.rotation, starOrigin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
