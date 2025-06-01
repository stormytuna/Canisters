using Canisters.Common;
using Canisters.DataStructures;
using Terraria.Audio;
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
		return new Vector2(-6f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(24, 24 * 2, 12f, 56, 6f);
		Item.width = 74;
		Item.height = 22;
		Item.SetShopValues(ItemRarityColor.Pink5, Item.buyPrice(gold: 5));

		Item.reuseDelay = 40;
		Item.consumeAmmoOnLastShotOnly = true;
		
		Item.crit = 2;
	}

	public override bool? UseItem(Player player) {
		var sound = SoundID.Item10 with { PitchRange = (0.1f, 0.4f), MaxInstances = 0 };
		SoundEngine.PlaySound(sound, player.Center);
		
		return base.UseItem(player);
	}
}

public class IcemireGlobalProjectile : ShotByWeaponGlobalProjectile<Icemire>
{
	public override bool ApplyFromParent() {
		return true;
	}

	public override void AI(Projectile projectile) {
		if (!IsActive || Main.rand.NextBool(4, 5)) {
			return;
		}

		Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Frost);
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
		return entity.type is NPCID.ArmoredViking or NPCID.IceTortoise or NPCID.IceElemental;
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Icemire>(), 50));
	}
}
