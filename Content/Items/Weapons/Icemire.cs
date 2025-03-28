using Canisters.Common;
using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Weapons;

public class Icemire : CanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(52f, 0f);
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 74;
		Item.height = 22;
		Item.value = Item.buyPrice(gold: 5);
		Item.rare = ItemRarityID.Pink;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = 24;
		Item.useAnimation = 24 * 3;
		Item.reuseDelay = 40;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<FiredVolatileCanister>();
		Item.shootSpeed = 15f;
		Item.damage = 16;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<VolatileCanister>();
		Item.consumeAmmoOnLastShotOnly = true;
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-10f, 0f);
	}

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
		ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.16f);
	}
}

public class IcemireGlobalProjectileLegacy : ShotByWeaponGlobalProjectileLegacy<Icemire>
{
	public override void AI(Projectile projectile) {
		if (!ShouldApply || projectile.hide || Main.rand.NextBool(4, 5)) {
			return;
		}

		var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Frost);
		dust.noGravity = true;
		dust.noLight = true;
	}

	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!ShouldApply || Main.rand.NextBool(2, 3)) {
			return;
		}

		target.AddBuff(BuffID.Frostburn2, 180);
	}
}

public class IcemireGlobalNpc : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.ArmoredViking || entity.type == NPCID.IceTortoise;
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Icemire>(), 50));
	}
}
