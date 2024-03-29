﻿using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Canisters.Common.DrawLayers;

/// <summary>
///     Allows canisters to have custom drawing without being held projectiles. Will need to refactor this when/if an API is added to do this
/// </summary>
public class CanisterWeaponDrawLayer : PlayerDrawLayer
{
	public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
		Player drawPlayer = drawInfo.drawPlayer;
		Item heldItem = drawPlayer.HeldItem;

		if (heldItem.ModItem is not CanisterUsingWeapon || !drawPlayer.ItemAnimationActive || drawPlayer.JustDroppedAnItem || drawInfo.shadow != 0f || drawPlayer.CCed || drawPlayer.dead) {
			return false;
		}

		return drawPlayer.HasAmmo(heldItem);
	}

	protected override void Draw(ref PlayerDrawSet drawInfo) {
		Player drawPlayer = drawInfo.drawPlayer;
		Item heldItem = drawPlayer.HeldItem;
		if (heldItem.ModItem is not CanisterUsingWeapon canisterWeapon || !drawPlayer.TryGetWeaponAmmo(heldItem, out int usedAmmoItemId)) {
			return;
		}

		Vector2 drawItemPos = Main.DrawPlayerItemPos(drawPlayer.gravDir, heldItem.type);
		Vector2 drawPosition = drawInfo.ItemLocation - Main.screenPosition;
		drawPosition = drawPosition.Floor();
		Rectangle itemDrawFrame = drawPlayer.GetItemDrawFrame(heldItem.type);
		float rotation = drawPlayer.itemRotation;
		float scale = drawPlayer.GetAdjustedItemScale(heldItem);
		float originX = drawPlayer.direction == -1 ? itemDrawFrame.Width + drawItemPos.X : -drawItemPos.X;
		Vector2 origin = new(originX, itemDrawFrame.Height / 2f);

		drawPosition += new Vector2(itemDrawFrame.Width / 2f, drawItemPos.Y);

		DrawData baseDrawData = new(canisterWeapon.BaseTexture.Value, drawPosition, itemDrawFrame, heldItem.GetAlpha(drawInfo.itemColor), rotation, origin, scale, drawInfo.itemEffect);
		drawInfo.DrawDataCache.Add(baseDrawData);

		DrawData canisterDrawData = baseDrawData with {
			texture = canisterWeapon.CanisterTexture.Value,
			color = CanisterHelpers.GetCanisterColor(usedAmmoItemId) * TileHelpers.GetBrightness(drawInfo.ItemLocation)
		};
		drawInfo.DrawDataCache.Add(canisterDrawData);
	}
}
