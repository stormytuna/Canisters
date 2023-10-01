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

	public override Vector2 MuzzleOffset => new(50f, -2f);

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
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 11f;
		Item.damage = 21;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}
}

public class BarkBellowerGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Dryad;

	public override void ModifyShop(NPCShop shop) {
		shop.Add<BarkBellower>(Condition.DownedEowOrBoc);
	}
}