using Canisters.Common;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Weapons;

public class Icemire : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(52f, 0f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-10f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(24, 24 * 2, 15f, 16, 3f);
		Item.width = 74;
		Item.height = 22;
		Item.SetShopValues(ItemRarityColor.Pink5, Item.buyPrice(gold: 5));
		Item.reuseDelay = 40;
		Item.consumeAmmoOnLastShotOnly = true;
	}
}

public class IcemireGlobalProjectile : ShotByWeaponGlobalProjectile<Icemire>
{
	public override void AI(Projectile projectile) {
		if (!IsActive || Main.rand.NextBool(4, 5)) {
			return;
		}

		var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Frost);
		dust.noGravity = true;
		dust.noLight = true;
	}

	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!IsActive || Main.rand.NextBool(2, 3)) {
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
