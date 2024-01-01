using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers.Abstracts;
using Canisters.Helpers.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

// TODO: Shit sprite!!!!
public class ChlorophyteSlugger : CanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType => CanisterFiringType.Depleted;

	public override Vector2 MuzzleOffset => new(38f, -2f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 50;
		Item.height = 24;
		Item.value = Item.buyPrice(gold: 5, silver: 50);
		Item.rare = ItemRarityID.Lime;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 8;
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
}
