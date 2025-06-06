using Canisters.DataStructures;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class QuantumFluxCannon : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Special;
	}

	public override Vector2 MuzzleOffset {
		get => new(70f, -4f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-8f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(26, 26, 12f, 104, 8f);
		Item.width = 66;
		Item.height = 28;
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.sellPrice(gold: 10));
		Item.UseSound = SoundID.NPCDeath56 with { PitchRange = (0.3f, 0.5f), Volume = 0.5f };
	}

	public override bool CanConsumeAmmo(Item ammo, Player player) {
		return Main.rand.NextBool(20, 100);
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Vector2 normal = velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * player.direction;

		_canisterFiringTypeOverride = CanisterFiringType.Depleted;
		base.Shoot(player, source, position + (normal * 5f), velocity, type, damage, knockback);

		_canisterFiringTypeOverride = CanisterFiringType.Launched;
		base.Shoot(player, source, position - (normal * 5f), velocity, type, damage, knockback);

		return false;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.FragmentVortex, 18)
			.AddTile(TileID.LunarCraftingStation)
			.Register();
	}
}
