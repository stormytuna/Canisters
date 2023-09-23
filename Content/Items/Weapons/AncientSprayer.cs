﻿using Canisters.Content.Projectiles.VolatileCanister;
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

	public override void SetDefaults() {
		// Base stats
		Item.width = 60;
		Item.height = 14;
		Item.value = Item.sellPrice(silver: 40);
		Item.rare = ItemRarityID.Blue;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 10;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.channel = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 9f;
		Item.damage = 11;
		Item.knockBack = 1f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.FossilOre, 15)
			.AddRecipeGroup(RecipeGroupID.IronBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}

// TODO: Visuals for this
public class AncientSprayerDrawLayer : PlayerDrawLayer
{
	private Asset<Texture2D> _texture;
	private Asset<Texture2D> Texture => _texture ??= ModContent.Request<Texture2D>("Canisters/Content/Items/Weapons/AncientSprayer_Back");

	public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Backpacks);

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<AncientSprayer>();

	protected override void Draw(ref PlayerDrawSet drawInfo) {
		Player drawPlayer = drawInfo.drawPlayer;

		Vector2 drawPosition = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height / 2);
		drawPosition.X += drawPlayer.direction * -13f;
		drawPosition.Y += drawPlayer.gravDir * 2f;
		drawPosition = drawPosition.Floor();
		Rectangle sourceRect = new(0, 0, Texture.Width(), Texture.Height());
		Vector2 origin = sourceRect.Size() / 2f;

		DrawData drawData = new(Texture.Value, drawPosition, sourceRect, drawInfo.colorArmorBody, drawPlayer.bodyRotation, origin, 1f, drawInfo.playerEffect);
		drawInfo.DrawDataCache.Add(drawData);
	}
}