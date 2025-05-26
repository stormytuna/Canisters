using Canisters.Common;
using Terraria.Enums;

namespace Canisters.Content.Items.Accessories;

public class SunBlessedTech : ModItem
{
	public override void SetDefaults() {
		Item.width = 30;
		Item.height = 30;
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.sellPrice(gold: 5));
		Item.accessory = true;
	}

	public override void UpdateAccessory(Player player, bool hideVisual) {
		player.GetModPlayer<SolarSigilPlayer>().Active = true;
		player.GetModPlayer<CanisterModifiersPlayer>().CanisterDepletedFireRateMult += 0.15f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<SolarSigil>()
			.AddIngredient<PneumaticPump>()
			.AddTile(TileID.TinkerersWorkbench)
			.Register();
	}
}
