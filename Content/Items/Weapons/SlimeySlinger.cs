﻿using Canisters.Common;
using Canisters.Content.Projectiles;
using Canisters.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Weapons;

public class SlimeySlinger : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(12f, -10f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(2f, -2f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(32, 32, 12f, 40, 6f);
		Item.width = 20;
		Item.height = 32;
		Item.SetShopValues(ItemRarityColor.Pink5, Item.buyPrice(gold: 1));
		Item.UseSound = SoundID.Item5 with { Pitch = 0.7f, PitchVariance = 0.1f };
	}
}

public class SlimySlingerGlobalProjectile : ShotByWeaponGlobalProjectile<SlimeySlinger>
{
	private int _numBounces = 1;

	public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
		if (!IsActive) {
			return base.OnTileCollide(projectile, oldVelocity);
		}

		_numBounces--;
		if (_numBounces < 0) {
			return true;
		}

		if (projectile.ModProjectile is BaseFiredCanisterProjectile canisterProjectile) {
			canisterProjectile.Explode(Main.player[projectile.owner]);
		}

		if (projectile.velocity.X != oldVelocity.X) {
			projectile.velocity.X = oldVelocity.X * -0.99f;
		}

		if (projectile.velocity.Y != oldVelocity.Y) {
			projectile.velocity.Y = oldVelocity.Y * -0.99f;
		}

		return false;
	}

	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!IsActive) {
			return;
		}

		_numBounces--;
		if (_numBounces < 0) {
			return;
		}

		if (projectile.ModProjectile is BaseFiredCanisterProjectile canisterProjectile) {
			canisterProjectile.Explode(Main.player[projectile.owner]);
		}

		// Would instantly kill infinite pierce projectiles otherwise
		if (projectile.penetrate >= 0) {
			projectile.penetrate++;
		}

		projectile.velocity = projectile.DirectionFrom(target.Center) * projectile.velocity.Length();
		projectile.netUpdate = true;
	}
}

public class SlimeySlingerGlobalNpc : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.QueenSlimeBoss;
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		LeadingConditionRule notExpertLeadingRule = new(new Conditions.NotExpert());
		notExpertLeadingRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SlimeySlinger>(), 4));
	}
}

public class SlimeySlingerGlobalItem : GlobalItem
{
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.QueenSlimeBossBag;
	}

	public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
		itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlimeySlinger>(), 4));
	}
}
