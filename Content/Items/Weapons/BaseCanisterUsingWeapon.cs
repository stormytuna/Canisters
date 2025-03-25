using System.Collections.Generic;
using System.Linq;
using Canisters.Content.Items.Canisters;
using Canisters.DataStructures;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using ReLogic.Content;
using Terraria.DataStructures;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Items.Weapons;

public abstract class BaseCanisterUsingWeapon : ModItem
{
	private Asset<Texture2D> _baseTexture;
	private Asset<Texture2D> _canisterTexture;

	public Asset<Texture2D> BaseTexture {
		get => _baseTexture ??= ModContent.Request<Texture2D>(Texture + "_Base");
	}

	public Asset<Texture2D> CanisterTexture {
		get => _canisterTexture ??= ModContent.Request<Texture2D>(Texture + "_Canister");
	}

	public abstract CanisterFiringType CanisterFiringType { get; }

	public virtual Vector2 MuzzleOffset {
		get => Vector2.Zero;
	}

	public virtual void ApplyWeaponStats(ref CanisterShootStats stats) { }

	public virtual IEnumerable<Projectile> ShootProjectiles(IEntitySource source, CanisterShootStats stats) {
		for (int i = 0; i < stats.ProjectileCount; i++) {
			Vector2 perturbedVelocity = i == 0
				? stats.Velocity // Forces one projectile to go towards cursor
				: stats.Velocity.RotatedByRandom(stats.TotalSpread);
			yield return Projectile.NewProjectileDirect(source, stats.Position, perturbedVelocity, stats.ProjectileType, stats.Damage, stats.Knockback, Main.myPlayer);
		}
	}

	public sealed override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		Vector2 muzzleOffset = velocity.SafeNormalize(Vector2.Zero) * MuzzleOffset.X;
		muzzleOffset += velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2) * player.direction * MuzzleOffset.Y;
		if (CollisionHelpers.CanHit(position, position + muzzleOffset)) {
			position += muzzleOffset;
		}
	}

	public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (ItemLoader.GetItem(source.AmmoItemIdUsed) is not BaseCanisterItem canister) {
			return true;
		}

		var stats = new CanisterShootStats {
			FiringType = CanisterFiringType,
			Velocity = velocity,
			Position = position,
			Damage = damage,
			Knockback = knockback,
			ProjectileCount = 1,
			ProjectileType = type,
			TotalSpread = 0f
		};
		canister.ApplyAmmoStats(ref stats);
		ApplyWeaponStats(ref stats);

		Projectile[] projectiles = ShootProjectiles(source, stats).ToArray();
		for (int i = 0; i < projectiles.Length; i++) {
			Projectile projectile = projectiles[i];
			canister.ModifyProjectile(projectile, i);
			if (Main.netMode != NetmodeID.SinglePlayer) {
				NetMessage.SendData(MessageID.SyncProjectile, ignoreClient: Main.myPlayer, number: projectile.whoAmI);
			}
		}

		return false;
	}

	public sealed override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		Player player = Main.LocalPlayer;
		if (!player.TryGetWeaponAmmo(Item, out int usedAmmoItemId)) {
			return true;
		}

		spriteBatch.Draw(BaseTexture.Value, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);

		Color canisterColor = CanisterHelpers.GetCanisterColor(usedAmmoItemId);
		spriteBatch.Draw(CanisterTexture.Value, position, frame, canisterColor, 0f, origin, scale, SpriteEffects.None, 0);

		return false;
	}
}
