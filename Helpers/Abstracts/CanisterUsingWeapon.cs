using Canisters.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace Canisters.Helpers.Abstracts;

/// <summary>
///     Handles weapons that visually show the canister
///     <para />
///     This class overrides these ModItem methods, so be sure to either call base or understand what each override does when overriding in your weapon
///     <list type="bullet">
///         <item>
///             <term>CanConsumeAmmo</term><description> Prevents the item from consuming ammo itself so only our held projectile will consume ammo</description>
///         </item>
///         <item>
///             <term>PreDrawInInventory</term><description> Draws the item with the canister coloured based on what canister the player will fire</description>
///         </item>
///     </list>
/// </summary>
public abstract class CanisterUsingWeapon : ModItem
{
	private Asset<Texture2D> _baseTexture;
	private Asset<Texture2D> BaseTexture => _baseTexture ??= ModContent.Request<Texture2D>(Texture + "_Base");

	private Asset<Texture2D> _canisterTexture;
	private Asset<Texture2D> CanisterTexture => _canisterTexture ??= ModContent.Request<Texture2D>(Texture + "_Canister");

	public override bool CanConsumeAmmo(Item ammo, Player player) => player.heldProj != -1;

	public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		// Check if we have any canisters
		Player player = Main.LocalPlayer;
		if (player.PickAmmo(Item, out _, out _, out _, out _, out int usedAmmoItemId, true)) {
			// Draw the weapon base
			spriteBatch.Draw(BaseTexture.Value, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);

			// Draw the canister
			Color canisterColor = CanisterColorSystem.GetCanisterColor(usedAmmoItemId);
			spriteBatch.Draw(CanisterTexture.Value, position, frame, canisterColor, 0f, origin, scale, SpriteEffects.None, 0);

			return false;
		}

		return true;
	}
}