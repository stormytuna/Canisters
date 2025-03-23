using System;
using Canisters.Helpers.Enums;
using ReLogic.Content;
using Terraria.DataStructures;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Helpers._Legacy.Abstracts;

/// <summary>
///     Handles weapons that use canisters
/// </summary>
public abstract class CanisterUsingWeapon : ModItem
{
	private Asset<Texture2D> _baseTexture;

	private Asset<Texture2D> _canisterTexture;

	public abstract CanisterFiringType CanisterFiringType { get; }

	public virtual Vector2 MuzzleOffset {
		get => Vector2.Zero;
	}

	public Asset<Texture2D> BaseTexture {
		get => _baseTexture ??= ModContent.Request<Texture2D>(Texture + "_Base");
	}

	public Asset<Texture2D> CanisterTexture {
		get => _canisterTexture ??= ModContent.Request<Texture2D>(Texture + "_Canister");
	}

	public virtual void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
		ref int damage, ref float knockback) {
	}

	public virtual void ApplyShootStats(ref Vector2 velocity, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread) {
	}

	public virtual void ShootProjectile(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int owner, float ai0, float ai1, float ai2) {
		Projectile.NewProjectile(source, position, velocity, type, damage, knockback, owner, ai0, ai1, ai2);
	}

	public sealed override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		// Check if we have any canisters
		Player player = Main.LocalPlayer;
		if (!player.PickAmmo(Item, out _, out _, out _, out _, out int usedAmmoItemId, true)) {
			return true;
		}

		// Draw the weapon base
		spriteBatch.Draw(BaseTexture.Value, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);

		// Draw the canister
		Color canisterColor = CanisterHelpers.GetCanisterColorLegacy(usedAmmoItemId);
		spriteBatch.Draw(CanisterTexture.Value, position, frame, canisterColor, 0f, origin, scale, SpriteEffects.None, 0);

		return false;
	}

	public sealed override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity,
		ref int type, ref int damage, ref float knockback) {
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
		Func<int, float[]> getAiCallback = null;
		canisterItem.ApplyAmmoStats(CanisterFiringType == CanisterFiringType.Launched, ref velocity, ref position, ref damage, ref knockback, ref amount, ref spread, ref getAiCallback);
		ApplyShootStats(ref velocity, ref position, ref damage, ref knockback, ref amount, ref spread);

		// Corrects our recoil simulation
		velocity = player.RotatedRelativePoint(player.MountedCenter).DirectionTo(Main.MouseWorld) * velocity.Length();

		for (int i = 0; i < amount; i++) {
			Vector2 perturbedVelocity = i == 0 ? velocity : velocity.RotatedByRandom(spread); // Forces one projectile to go towards the cursor
			float[] ai = getAiCallback?.Invoke(i) ?? [0f, 0f, 0f];
			ShootProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI, ai[0], ai[1], ai[2]);
		}

		return false;
	}
}
