using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class Icemire : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Launched;

	public override Vector2 MuzzleOffset => new(52f, 0f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 50;
		Item.height = 22;
		Item.value = Item.sellPrice(gold: 4);
		Item.rare = ItemRarityID.LightRed;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = 24;
		Item.useAnimation = 24 * 3;
		Item.reuseDelay = 40;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 15f;
		Item.damage = 16;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
		Item.consumeAmmoOnLastShotOnly = true;
	}

	public override Vector2? HoldoutOffset() => new Vector2(-10f, 0f);

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.16f);
	}
}

public class IcemireGlobalProjectile : ShotByWeaponGlobalProjectile<Icemire>
{
	// TODO: Dust when travelling

	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!ShouldApply || Main.rand.NextBool(2, 3)) {
			return;
		}

		target.AddBuff(BuffID.Frostburn2, 180);
	}
}

public class IcemireGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.ArmoredViking || entity.type == NPCID.IceTortoise;

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Icemire>(), 50));
	}
}