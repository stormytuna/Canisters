using Canisters.Common;
using Terraria.Enums;

namespace Canisters.Content.Items.Accessories;

public class CryoPumpAssembly : ModItem
{
	public override void SetDefaults() {
		Item.width = 34;
		Item.height = 34;
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.sellPrice(gold: 5));
		Item.accessory = true;
	}

	public override void UpdateAccessory(Player player, bool hideVisual) {
		player.GetModPlayer<ArcticCoolantPlayer>().Active = true;
		player.GetModPlayer<ArcticCoolantPlayer>().Item = Item;
		player.GetModPlayer<CanisterModifiersPlayer>().CanisterLaunchedExplosionRadiusMult += 0.5f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<ArcticCoolant>()
			.AddIngredient<PneumaticPump>()
			.AddTile(TileID.TinkerersWorkbench)
			.Register();
	}
}
