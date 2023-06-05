using System;
using System.IO;
using Canisters.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Helpers.Abstracts;

/// <summary>
///     Handles projectile held weapons for us
///     <para />
///     Be sure to set these helper properties in SetDefaults hook
///     <para />
///     <list type="bullet">
///         <item>
///             <term>HoldOutOffset</term><description> How far away the projectile will display from your character</description>
///         </item>
///         <item>
///             <term>CanisterFiringType</term><description> The firing type, either Canister or Regular</description>
///         </item>
///         <item>
///             <term>RotationOffset</term><description> How many extra radians this projectile will rotate when it's pointed to the mouse</description>
///         </item>
///         <item>
///             <term>Texture</term><description> This is actually provided by ModProjectile, but make sure to override it and set it to the items sprite filepath</description>
///         </item>
///     </list>
///     This class overrides these ModProjectile methods, so be sure to either call base or understand what each override does when overriding in your weapon
///     <list type="bullet">
///         <item>
///             <term>AI</term><description> Handles projectile direction, location and rotation and sets some values on the player</description>
///         </item>
///         <item>
///             <term>PreDraw</term><description> Handles drawing the canister colour</description>
///         </item>
///         <item>
///             <term>SendExtraAI and ReceiveExtraAI</term><description> Handles sending and receiving AI fields created by this class</description>
///         </item>
///     </list>
///     This class provides these virtual methods
///     <list type="bullet">
///         <item>
///             <term>Shoot</term><description> This is called each time the projectile shoots</description>
///         </item>
///     </list>
/// </summary>
public abstract class CanisterUsingHeldProjectile : ModProjectile
{
	/// <summary>Returns Main.player[Projectile.owner]</summary>
	public Player Owner => Main.player[Projectile.owner];

	/// <summary>How far away this projectile will appear from the player</summary>
	public float HoldOutOffset { get; set; }

	/// <summary>The firing type this weapon uses, either Canister or Regular</summary>
	public FiringType CanisterFiringType { get; set; }

	/// <summary>How much this projectile should be rotated when it points to the mouse</summary>
	public float RotationOffset { get; set; }

	/// <summary>How far from the center the projectile will be fired from, assuming the projectile looks like its sprite (facing to the right)</summary>
	public Vector2 MuzzleOffset { get; set; }

	/// <summary>This property acts as a frame counter</summary>
	public int AI_FrameCount { get; set; }

	/// <summary>The sound this held projectile will play when shooting a projectile</summary>
	public SoundStyle? ShootSound { get; set; } = null;

	private int AI_Lifetime { get; set; }

	// Helper property that applies attack speed for us
	public int UseTimeAfterBuffs => (int)(Owner.HeldItem.useTime * CombinedHooks.TotalUseTimeMultiplier(Owner, Owner.HeldItem));

	public virtual void Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }

	public override void AI() {
		Vector2 toMouse = Main.MouseWorld - Projectile.Center;
		toMouse.Normalize();
		bool hasAmmo = Owner.PickAmmo(Owner.HeldItem, out _, out _, out _, out _, out _, true);
		bool doShoot = false; // Used so we can delay the calling of our actual shoot hook since we need rotation set early

		// Kill the projectile if we stop using it or can't use it
		if (((!Owner.channel || !hasAmmo) && AI_Lifetime <= 1) || Owner.CCed) {
			Projectile.Kill();
			return;
		}

		// Set our rotation and doShoot if we are shooting this frame
		if (AI_FrameCount % UseTimeAfterBuffs == 0) {
			// Set rotation
			Projectile.rotation = toMouse.ToRotation() + RotationOffset;
			doShoot = true;
		}

		// Set some stuff based on our rotation
		Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + Projectile.rotation.ToRotationVector2() * HoldOutOffset;
		Projectile.velocity = Vector2.Zero;
		Projectile.direction = Math.Sign(Projectile.Center.X - Owner.Center.X);
		Projectile.spriteDirection = Projectile.direction;

		// Set some values on our player
		Owner.ChangeDir(Projectile.direction);
		Owner.heldProj = Projectile.whoAmI;
		Owner.itemRotation = Projectile.DirectionFrom(Owner.MountedCenter).ToRotation();
		if (Projectile.Center.X < Owner.MountedCenter.X) {
			Owner.itemRotation += (float)Math.PI;
		}

		Owner.itemRotation = MathHelper.WrapAngle(Owner.itemRotation);

		// Actually call our shoot hook
		if (doShoot) {
			Owner.PickAmmo(Owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockback, out int usedAmmoItemId);

			// Get our projectile type
			CanisterItem canisterItem = ContentSamples.ItemsByType[usedAmmoItemId].ModItem as CanisterItem;
			if (CanisterFiringType == FiringType.Canister) {
				projToShoot = canisterItem.LaunchedProjectileType;
			} else {
				projToShoot = canisterItem.DepletedProjectileType;
			}

			// Get some other params
			EntitySource_ItemUse_WithAmmo source = new(Owner, Owner.HeldItem, usedAmmoItemId);
			Vector2 velocity = toMouse * speed;
			Vector2 offset = new(MuzzleOffset.X, MuzzleOffset.Y * Projectile.direction);
			offset = offset.RotatedBy(Projectile.rotation);
			Vector2 position = Projectile.Center + offset;
			int amount = 1;
			float spread = 0f;

			canisterItem.ApplyAmmoStats(CanisterFiringType == FiringType.Canister, ref velocity, ref offset, ref position, ref damage, ref knockback, ref amount, ref spread);

			for (int i = 0; i < amount; i++) {
				Vector2 perturbedVelocity = velocity.RotatedByRandom(spread);
				Shoot(Owner, source, position, perturbedVelocity, projToShoot, damage, knockback);
			}

			// Play our sound
			SoundStyle? shootSound = CanisterFiringType == FiringType.Canister ? ShootSound : CanisterSoundSystem.GetDepletedCanisterSound(usedAmmoItemId);
			if (shootSound is not null) {
				SoundEngine.PlaySound(shootSound, Projectile.Center);
			}

			// This makes it so we keep our item used until the potential next shot
			Owner.SetDummyItemTime(UseTimeAfterBuffs + 1);

			AI_Lifetime = UseTimeAfterBuffs + 1;
			Projectile.netUpdate = true;
		}

		// Set timeleft
		Projectile.timeLeft = 2;

		AI_FrameCount++;
		AI_Lifetime--;

		base.AI();
	}

	private Asset<Texture2D> _baseTexture;
	private Asset<Texture2D> BaseTexture => _baseTexture ??= ModContent.Request<Texture2D>(Texture + "_Base");

	private Asset<Texture2D> _canisterTexture;
	private Asset<Texture2D> CanisterTexture => _canisterTexture ??= ModContent.Request<Texture2D>(Texture + "_Canister");

	public override bool PreDraw(ref Color lightColor) {
		if (Owner.PickAmmo(Owner.HeldItem, out _, out _, out _, out _, out int usedAmmoItemID, true)) {
			Vector2 position = Projectile.Center - Main.screenPosition;
			Rectangle frame = new(0, 0, BaseTexture.Width(), BaseTexture.Height());
			Color drawColor = lightColor;
			float rotation = Projectile.rotation;
			Vector2 origin = frame.Size() / 2f;
			float scale = Projectile.scale;
			SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

			Main.EntitySpriteDraw(BaseTexture.Value, position, frame, drawColor, rotation, origin, scale, effects);

			Point projectileTileCoords = Projectile.Center.ToTileCoordinates();
			Color canisterColor = CanisterColorSystem.GetCanisterColor(usedAmmoItemID) * Lighting.Brightness(projectileTileCoords.X, projectileTileCoords.Y);
			canisterColor.A = 255;
			Main.EntitySpriteDraw(CanisterTexture.Value, position, frame, canisterColor, rotation, origin, scale, effects);

			return false;
		}

		return base.PreDraw(ref lightColor);
	}

	// Sends and receives our ai fields, not even sure if we need this but w/e
	public override void SendExtraAI(BinaryWriter writer) {
		writer.Write(AI_FrameCount);
		writer.Write(AI_Lifetime);

		base.SendExtraAI(writer);
	}

	public override void ReceiveExtraAI(BinaryReader reader) {
		AI_FrameCount = reader.ReadInt32();
		AI_Lifetime = reader.ReadInt32();

		base.ReceiveExtraAI(reader);
	}
}