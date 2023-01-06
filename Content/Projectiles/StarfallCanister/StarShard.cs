using Terraria;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.StarfallCanister
{
    /// <summary>
    ///     Star shard that the splitting star fires when it 'explodes'
    /// </summary>
    public class StarShard : ModProjectile
    {
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 14;
            Projectile.height = 10;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        public override void AI() {
            // Point in direction of velocity 
            Projectile.rotation = Projectile.velocity.ToRotation();

            base.AI();
        }
    }
}
