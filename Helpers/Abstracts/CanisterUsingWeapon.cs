using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Helpers.Abstracts;

/// <summary>
///     Handles weapons that use canisters
/// </summary>
public abstract class CanisterUsingWeapon : ModItem
{
	public virtual void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
	public virtual void ApplyShootStats(ref Vector2 velocity, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread) { }
	public virtual void ShootProjectile(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int owner) => Projectile.NewProjectile(source, position, velocity, type, damage, knockback, owner);

	public abstract FiringType FiringType { get; }

	public virtual Vector2 MuzzleOffset => Vector2.Zero;

	private Asset<Texture2D> _baseTexture;
	public Asset<Texture2D> BaseTexture => _baseTexture ??= ModContent.Request<Texture2D>(Texture + "_Base");

	private Asset<Texture2D> _canisterTexture;
	public Asset<Texture2D> CanisterTexture => _canisterTexture ??= ModContent.Request<Texture2D>(Texture + "_Canister");

	public sealed override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		// Check if we have any canisters
		Player player = Main.LocalPlayer;
		if (!player.PickAmmo(Item, out _, out _, out _, out _, out int usedAmmoItemId, true)) {
			return true;
		}

		// Draw the weapon base
		spriteBatch.Draw(BaseTexture.Value, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);

		// Draw the canister
		Color canisterColor = CanisterHelpers.GetCanisterColor(usedAmmoItemId);
		spriteBatch.Draw(CanisterTexture.Value, position, frame, canisterColor, 0f, origin, scale, SpriteEffects.None, 0);

		return false;
	}

	public sealed override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		SafeModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);

		Vector2 muzzleOffset = velocity.SafeNormalize(Vector2.Zero) * MuzzleOffset.X;
		muzzleOffset += velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2) * player.direction * MuzzleOffset.Y;
		if (CollisionHelpers.CanHit(position, position + muzzleOffset)) {
			position += muzzleOffset;
		}
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (ItemLoader.GetItem(source.AmmoItemIdUsed) is not CanisterItem canisterItem) {
			return true;
		}

		int amount = 1;
		float spread = 0f;
		canisterItem.ApplyAmmoStats(FiringType == FiringType.Launched, ref velocity, ref position, ref damage, ref knockback, ref amount, ref spread);
		ApplyShootStats(ref velocity, ref position, ref damage, ref knockback, ref amount, ref spread);

		// Corrects our recoil simulation
		velocity = player.RotatedRelativePoint(player.MountedCenter).DirectionTo(Main.MouseWorld) * velocity.Length();

		for (int i = 0; i < amount; i++) {
			Vector2 perturbedVelocity = i == 0 ? velocity : velocity.RotatedByRandom(spread); // Forces one projectile to go towards the cursor
			ShootProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI);
		}

		return false;
	}
}
