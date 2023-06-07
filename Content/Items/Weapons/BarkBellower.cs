using Canisters.Content.Items.Canisters;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class BarkBellower : CanisterUsingWeapon
{
	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 44;
		Item.height = 20;
		Item.value = Item.buyPrice(gold: 15);
		Item.rare = ItemRarityID.Green;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 16;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.channel = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<BarkBellower_HeldProjectile>();
		Item.shootSpeed = 11f;
		Item.damage = 21;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<VolatileCanister>();
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<BarkBellower_HeldProjectile>(), damage, knockback, player.whoAmI);

		return false;
	}
}

public class BarkBellower_HeldProjectile : CanisterUsingHeldProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 44;
		Projectile.height = 20;
		Projectile.aiStyle = -1;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;

		// Held projectile stats
		Projectile.tileCollide = false;
		Projectile.hide = true;
		Projectile.ignoreWater = true;

		// CanisterUsingHeldProjectile stats
		HoldOutOffset = 16f;
		CanisterFiringType = FiringType.Depleted;
		RotationOffset = 0f;
		MuzzleOffset = new Vector2(18f, -2f);
		TotalRandomSpread = 0.1f;
	}

	public override string Texture => "Canisters/Content/Items/Weapons/BarkBellower";
}

public class BarkBellowerGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Dryad;

	public override void ModifyShop(NPCShop shop) {
		shop.Add<BarkBellower>(Condition.DownedEowOrBoc);
	}
}