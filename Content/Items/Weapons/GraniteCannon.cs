using System.Linq;
using Canisters.Content.Items.Canisters;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class GraniteCannon : CanisterUsingWeapon
{
	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 44;
		Item.height = 16;
		Item.value = Item.sellPrice(silver: 30);
		Item.rare = ItemRarityID.Blue;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 26;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.channel = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<GraniteCannon_HeldProjectile>();
		Item.shootSpeed = 11f;
		Item.damage = 16;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<VolatileCanister>();
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<GraniteCannon_HeldProjectile>(), damage, knockback, player.whoAmI);

		return false;
	}
}

public class GraniteCannon_HeldProjectile : CanisterUsingHeldProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 44;
		Projectile.height = 16;
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
		HoldOutOffset = 14f;
		CanisterFiringType = FiringType.Launched;
		RotationOffset = 0f;
		MuzzleOffset = new Vector2(16f, -6f);
		TotalRandomSpread = 0.1f;
	}

	public override string Texture => "Canisters/Content/Items/Weapons/GraniteCannon";
}

public class GraniteCannonGlobalNPC : GlobalNPC
{
	private static readonly int[] GraniteEnemies = { NPCID.GraniteFlyer, NPCID.GraniteGolem };

	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => GraniteEnemies.Contains(entity.type);

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraniteCannon>(), 20));
	}
}