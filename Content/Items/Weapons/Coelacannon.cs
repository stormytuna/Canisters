using Canisters.Common;
using Canisters.Content.Projectiles;
using Canisters.DataStructures;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class Coelacannon : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(38f, 12f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-8f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(32, 32, 9f, 86, 6f);
		Item.width = 72;
		Item.height = 36;
		Item.SetShopValues(ItemRarityColor.Lime7, Item.buyPrice(gold: 8));
		Item.UseSound = SoundID.Item10 with { PitchRange = (0.8f, 1f), MaxInstances = 0 };
	}
}

public class CoelacannonGlobalProjectile : ShotByWeaponGlobalProjectile<Coelacannon>
{
	private static Asset<Texture2D> _fishBaseTexture;
	private static Asset<Texture2D> _fishCanisterTexture;

	private bool _firstFrame = true;
	private float _maxSpeed;
	private NPC _target;
	private int _timeUntilActive = 10;

	public override bool ApplyFromParent() {
		return false;
	}

	public override void Load() {
		_fishBaseTexture = Mod.Assets.Request<Texture2D>("Content/Items/Weapons/Coelacannon_Fish_Base");
		_fishCanisterTexture = Mod.Assets.Request<Texture2D>("Content/Items/Weapons/Coelacannon_Fish_Canister");
	}

	public override bool PreAI(Projectile projectile) {
		if (!IsActive || projectile.ModProjectile is not BaseFiredCanisterProjectile) {
			return true;
		}

		if (_firstFrame) {
			_firstFrame = false;
			_maxSpeed = projectile.velocity.Length();
		}

		if (_timeUntilActive > 0) {
			_timeUntilActive--;
			return true;
		}

		if (_target is null || !_target.active) {
			_target = NPCHelpers.FindClosestNPC(25f * 16f, projectile.Center);
			if (_target is null) {
				return true;
			}
		}

		projectile.rotation = projectile.velocity.ToRotation();
		MathHelpers.SmoothHoming(projectile, _target.Center, 0.5f, _maxSpeed, _target.velocity);

		return false;
	}

	public override bool PreDraw(Projectile projectile, ref Color lightColor) {
		if (!IsActive || projectile.ModProjectile is not BaseFiredCanisterProjectile) {
			return true;
		}

		DrawData baseDrawData = new() {
			texture = _fishBaseTexture.Value,
			position = (projectile.Center - Main.screenPosition).Floor(),
			sourceRect = _fishBaseTexture.Frame(),
			color = lightColor,
			rotation = projectile.rotation,
			scale = new Vector2(projectile.scale),
			origin = _fishBaseTexture.Size() / 2f,
			effect = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None
		};
		DrawData canisterDrawData = baseDrawData with {
			texture = _fishCanisterTexture.Value,
			color = Lighting.GetColor(projectile.Center.ToTileCoordinates(), CanisterHelpers.GetCanisterColor(ShotByCanisterType)),
		};

		baseDrawData.Draw(Main.spriteBatch);
		canisterDrawData.Draw(Main.spriteBatch);

		return false;
	}
}

public class CoelacannonPlayer : ModPlayer
{
	public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition) {
		if (Player.ZoneTowerVortex) {
			itemDrop = ModContent.ItemType<Coelacannon>();
		}
	}
}
