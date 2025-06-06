using Canisters.Common;
using Terraria.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Accessories;

public class SolarSigil : ModItem
{
	public override void SetDefaults() {
		Item.width = 28;
		Item.height = 28;
		Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 4));
		Item.accessory = true;
	}

	public override void UpdateAccessory(Player player, bool hideVisual) {
		player.GetModPlayer<SolarSigilPlayer>().Active = true;
	}
}

public class SolarSigilPlayer : ModPlayer
{
	public bool Active = false;

	public override void ResetEffects() {
		Active = false;
	}

	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!hit.Crit || !Active) {
			return;
		}

		if (!proj.GetGlobalProjectile<CanistersProjectileTracker>().IsDepletedCanisterProjectile) {
			return;
		}

		NPC laserTarget = NPCHelpers.FindRandomNearbyNPC(20f * 16f, target.Center, true, [target.whoAmI]);
		if (laserTarget is null) {
			return;
		}

		Main.LocalPlayer.ApplyDamageToNPC(laserTarget, hit.Damage / 6, 0f, 1, false, DamageClass.Ranged, true);

		Vector2 dustStart = Main.rand.NextVectorWithin(target.Hitbox);
		Vector2 dustEnd = Main.rand.NextVectorWithin(laserTarget.Hitbox);

		float length = (dustEnd - dustStart).Length();
		for (float i = Main.rand.NextFloat(4f); i < length; i += 6f) {
			Vector2 offset = dustStart.DirectionTo(dustEnd) * i;
			Dust dust = Dust.NewDustPerfect(dustStart + offset, DustID.SolarFlare);
			dust.noGravity = true;
			dust.velocity *= 0.1f;
			dust.rotation = Main.rand.NextRadian();
			dust.scale = Main.rand.NextFloat(0.6f, 0.8f);
		}
	}
}

public class SolarSigilGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type is NPCID.Lihzahrd or NPCID.LihzahrdCrawler or NPCID.FlyingSnake;
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SolarSigil>(), 80));
	}
}
