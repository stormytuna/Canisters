using Canisters.DataStructures;
using Canisters.Helpers;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class AncientSprayer : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(48f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(0f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(10, 10, 8f, 10, 0f);
		Item.width = 60;
		Item.height = 14;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 40));
		Item.UseSound = SoundID.Item13 with { PitchVariance = 0.1f };
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.Velocity = stats.Velocity.RotatedByRandom(0.4f);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.FossilOre, 15)
			.AddRecipeGroup(RecipeGroupID.IronBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}

public class AncientSprayerDrawLayer : PlayerDrawLayer
{
	private Asset<Texture2D> _baseTexture;
	private Asset<Texture2D> _canisterTexture;

	private Asset<Texture2D> BaseTexture {
		get => _baseTexture ??=
			ModContent.Request<Texture2D>("Canisters/Content/Items/Weapons/AncientSprayer_Back_Base");
	}

	private Asset<Texture2D> CanisterTexture {
		get => _canisterTexture ??=
			ModContent.Request<Texture2D>("Canisters/Content/Items/Weapons/AncientSprayer_Back_Canister");
	}

	public override Position GetDefaultPosition() {
		return new AfterParent(PlayerDrawLayers.Backpacks);
	}

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
		return drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<AncientSprayer>();
	}

	protected override void Draw(ref PlayerDrawSet drawInfo) {
		Player drawPlayer = drawInfo.drawPlayer;

		Vector2 drawPosition = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2, drawPlayer.height - (drawPlayer.bodyFrame.Height / 2));
		drawPosition.X += drawPlayer.direction * -15f;
		int bodyFrame = drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height;
		if (bodyFrame is (>= 7 and <= 9) or (>= 14 and <= 16)) {
			// Offset for when player is "taller"
			drawPosition.Y -= 2f;
		}

		drawPosition = drawPosition.Floor();

		Rectangle sourceRect = new(0, 0, BaseTexture.Width(), BaseTexture.Height());
		Vector2 origin = sourceRect.Size() / 2f;

		DrawData drawData = new(BaseTexture.Value, drawPosition, sourceRect, drawInfo.colorArmorBody, drawPlayer.bodyRotation, origin, 1f, drawInfo.playerEffect);
		drawInfo.DrawDataCache.Add(drawData);

		Color canisterColor = Lighting.GetColor(drawInfo.Position.ToTileCoordinates(), CanisterHelpers.GetCanisterColorForHeldWeapon(drawPlayer));
		drawInfo.DrawDataCache.Add(drawData with { texture = CanisterTexture.Value, color = canisterColor });
	}
}
