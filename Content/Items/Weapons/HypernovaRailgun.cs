using Canisters.Common;
using Canisters.Content.Projectiles;
using Canisters.DataStructures;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Items.Weapons;

public class HypernovaRailgun : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(20f, -4f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-4f, -2f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(26, 26, 12f, 60, 8f);
		Item.width = 66;
		Item.height = 28;
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.sellPrice(gold: 10));
		Item.UseSound = SoundID.NPCDeath44 with { PitchRange = (0.5f, 0.8f) };
		
		Item.crit = 11;
	}
}

public class HypernovaRailgunGlobalProjectile : ShotByWeaponGlobalProjectile<HypernovaRailgun>
{
	private const float MaxExtraDamage = 0.2f;
	private const int MinFramesForExtraDamage = 30;
	private const int MaxFramesForExtraDamage = 50;

	private int _framesTravelling = 0;

	public override void SafeOnSpawn(Projectile projectile, IEntitySource source) {
		if (IsActive) {
			projectile.extraUpdates = 8;
		}
	}

	public override bool PreAI(Projectile projectile) {
		if (!IsActive || projectile.ModProjectile is not BaseFiredCanisterProjectile) {
			return true;
		}

		projectile.rotation = projectile.velocity.ToRotation() + PiOver4;

		_framesTravelling++;

		return false;
	}

	public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
		if (!IsActive || _framesTravelling <= MinFramesForExtraDamage) {
			return;
		}

		float progress = (float)(_framesTravelling - MinFramesForExtraDamage) / (MaxFramesForExtraDamage - MinFramesForExtraDamage);
		float extraDamage = float.Min(progress * MaxExtraDamage, MaxExtraDamage);
		modifiers.FinalDamage += extraDamage;
	}

	public override bool PreDraw(Projectile projectile, ref Color lightColor) {
		if (!IsActive || projectile.ModProjectile is not BaseFiredCanisterProjectile) {
			return true;
		}

		Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
		Vector2 origin = texture.Size() / 2f;
		for (int i = 20; i >= 0; i--) {
			float progress = i / 20f;
			Vector2 drawPos = (projectile.Center - (projectile.velocity * progress * 3f) - Main.screenPosition).Floor();
			Main.EntitySpriteDraw(texture, drawPos, null, projectile.GetAlpha(lightColor) * float.Lerp(0.2f, 1f, 1 - progress), projectile.rotation, origin, projectile.scale * float.Lerp(0.8f, 1f, float.Pow(1 - progress, 2)), SpriteEffects.None, 0f);
		}

		return false;
	}
}

public class HypernovaRailgunGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type is NPCID.BrainScrambler or NPCID.RayGunner or NPCID.MartianOfficer or NPCID.GrayGrunt or NPCID.MartianEngineer or NPCID.GigaZapper or NPCID.ScutlixRider or NPCID.MartianWalker;
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HypernovaRailgun>(), 800));
	}
}
