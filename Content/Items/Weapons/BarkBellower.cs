using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class BarkBellower : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Depleted;

	public override Vector2 MuzzleOffset => new(38f, -2f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 44;
		Item.height = 20;
		Item.value = Item.sellPrice(gold: 15);
		Item.rare = ItemRarityID.Green;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 16;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 11f;
		Item.damage = 21;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override Vector2? HoldoutOffset() => new Vector2(-2f, 0f);

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.15f);
	}
}

public class BarkBellowerGlobalProjectile : ShotByWeaponGlobalProjectile<BarkBellower>
{
    public override void AI(Projectile projectile) {
        if (!ShouldApply || projectile.hide || Main.rand.NextBool(4, 5)) {
            return;
        }

        Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.DryadsWard);
        dust.noGravity = true;
        dust.noLight = true;
    }

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
        if (!ShouldApply || Main.rand.NextBool(2, 3)) {
            return;
        }

        target.AddBuff(BuffID.DryadsWardDebuff, 180);
    }
}

public class BarkBellowerGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Dryad;

	public override void ModifyShop(NPCShop shop) {
		shop.Add<BarkBellower>(Condition.DownedEowOrBoc);
	}
}
