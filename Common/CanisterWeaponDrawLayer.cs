using Canisters.Content.Items.Weapons;
using Canisters.Helpers;
using Terraria.DataStructures;

namespace Canisters.Common;

public class CanisterWeaponDrawLayer : PlayerDrawLayer
{
	public override Position GetDefaultPosition() {
		return new AfterParent(PlayerDrawLayers.HeldItem);
	}

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
		Player drawPlayer = drawInfo.drawPlayer;
		Item heldItem = drawPlayer.HeldItem;

		if (heldItem.ModItem is not BaseCanisterUsingWeapon || !drawPlayer.ItemAnimationActive ||
			drawPlayer.JustDroppedAnItem || drawInfo.shadow != 0f || drawPlayer.CCed || drawPlayer.dead) {
			return false;
		}

		return drawPlayer.HasAmmo(heldItem);
	}

	protected override void Draw(ref PlayerDrawSet drawInfo) {
		Player drawPlayer = drawInfo.drawPlayer;
		Item heldItem = drawPlayer.HeldItem;
		if (heldItem.ModItem is not BaseCanisterUsingWeapon canisterWeapon || !drawPlayer.TryGetWeaponAmmo(heldItem, out int usedAmmoItemId)) {
			return;
		}

		Vector2 drawItemPos = Main.DrawPlayerItemPos(drawPlayer.gravDir, heldItem.type);
		Vector2 drawPosition = drawPlayer.itemLocation - Main.screenPosition;
		drawPosition += new Vector2(drawItemPos.X * drawPlayer.direction, drawItemPos.Y);
		drawPosition = drawPosition.Floor();
		Rectangle itemDrawFrame = drawPlayer.GetItemDrawFrame(heldItem.type);
		float rotation = drawPlayer.itemRotation;
		float scale = drawPlayer.GetAdjustedItemScale(heldItem);
		float originX = drawPlayer.direction == -1 ? itemDrawFrame.Width + drawItemPos.X : -drawItemPos.X;
		Vector2 origin = new(originX, itemDrawFrame.Height / 2f);

		DrawData baseDrawData = new(canisterWeapon.BaseTexture.Value, drawPosition, itemDrawFrame,
			heldItem.GetAlpha(drawInfo.itemColor), rotation, origin, scale, drawInfo.itemEffect);
		drawInfo.DrawDataCache.Add(baseDrawData);

		DrawData canisterDrawData = baseDrawData with { texture = canisterWeapon.CanisterTexture.Value, color = CanisterHelpers.GetCanisterColor(usedAmmoItemId) * TileHelpers.GetBrightness(drawInfo.ItemLocation) };
		drawInfo.DrawDataCache.Add(canisterDrawData);
	}
}
