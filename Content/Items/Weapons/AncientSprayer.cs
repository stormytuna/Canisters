using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class AncientSprayer : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Depleted;

	public override Vector2 MuzzleOffset => new(52f, 0f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 60;
		Item.height = 14;
		Item.value = Item.buyPrice(silver: 40);
		Item.rare = ItemRarityID.Blue;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 10;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 9f;
		Item.damage = 11;
		Item.knockBack = 1f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override Vector2? HoldoutOffset() => new Vector2(-6f, 0f);

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.4f);
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
	private Asset<Texture2D> BaseTexture => _baseTexture ??= ModContent.Request<Texture2D>("Canisters/Content/Items/Weapons/AncientSprayer_Back_Base");
	private Asset<Texture2D> _canisterTexture;
	private Asset<Texture2D> CanisterTexture => _canisterTexture ??= ModContent.Request<Texture2D>("Canisters/Content/Items/Weapons/AncientSprayer_Back_Canister");

	public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Backpacks);

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<AncientSprayer>();

	protected override void Draw(ref PlayerDrawSet drawInfo) {
		Player drawPlayer = drawInfo.drawPlayer;

		Vector2 drawPosition = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height / 2);
		drawPosition.X += drawPlayer.direction * -15f;
		drawPosition.Y += drawPlayer.gravDir * 3f;
		drawPosition = drawPosition.Floor();
		Rectangle sourceRect = new(0, 0, BaseTexture.Width(), BaseTexture.Height());
		Vector2 origin = sourceRect.Size() / 2f;

		DrawData drawData = new(BaseTexture.Value, drawPosition, sourceRect, drawInfo.colorArmorBody, drawPlayer.bodyRotation, origin, 1f, drawInfo.playerEffect);
		drawInfo.DrawDataCache.Add(drawData);

		Color canisterColor = CanisterHelpers.GetCanisterColorForHeldItem(drawPlayer);
		drawInfo.DrawDataCache.Add(drawData with {
			texture = CanisterTexture.Value,
			color = canisterColor
		});
	}
}