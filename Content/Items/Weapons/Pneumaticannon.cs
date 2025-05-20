using System.Collections.Generic;
using System.IO;
using Canisters.Common;
using Canisters.Content.Projectiles.GhastlyCanister;
using Canisters.Content.Projectiles.LunarCanister;
using Canisters.Content.Projectiles.NaniteCanister;
using Canisters.Content.Projectiles.ToxicCanister;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader.IO;

namespace Canisters.Content.Items.Weapons;

public class Pneumaticannon : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(22f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(0f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(28, 28, 15f, 50, 3f);
		Item.width = 48;
		Item.height = 14;
		Item.SetShopValues(ItemRarityColor.Pink5, Item.sellPrice(gold: 75));
		Item.UseSound = SoundID.Item10 with { PitchRange = (-0.6f, -0.4f) };
	}
}

public class PneumaticannonGlobalProjectile : ShotByWeaponGlobalProjectile<Pneumaticannon>
{
	// TODO: move somewhere else ?
	public static HashSet<int> ExemptProjectiles = new();

	public override void SetStaticDefaults() {
		ExemptProjectiles.UnionWith([
			ModContent.ProjectileType<ToxicFog>(),
			ModContent.ProjectileType<Nanites>(),
			ModContent.ProjectileType<GhastlyExplosionEmitter>(),
			ModContent.ProjectileType<LunarLightningEmitter>(),
		]);
	}

	public override void SafeOnSpawn(Projectile projectile, IEntitySource source) {
		bool notExempt = ServerConfig.Instance.AllowExtraUpdatesOnWeirdProjectiles || !ExemptProjectiles.Contains(projectile.type);
		if (IsActive && notExempt) {
			projectile.extraUpdates = 2;
		}
	}
}

public class PneumaticannonGlobalNpc : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Steampunker;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.Add<Pneumaticannon>(Condition.DownedMechBossAll);
	}
}
