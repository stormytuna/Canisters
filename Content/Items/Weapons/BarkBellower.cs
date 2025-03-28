using Canisters.Common;
using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Terraria.Enums;
using Terraria.GameContent;

namespace Canisters.Content.Items.Weapons;

public class BarkBellower : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(38f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-2f, 0f);
	}
	
	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(16, 16, 11f, 21, 3f);
		Item.width = 44;
		Item.height = 20;
		Item.SetShopValues(ItemRarityColor.Green2, Item.sellPrice(gold: 15));
	}
}

public class BarkBellowerGlobalProjectile : ShotByWeaponGlobalProjectile<BarkBellower>
{
	public override void AI(Projectile projectile) {
		if (!IsActive || projectile.hide || Main.rand.NextBool(4, 5)) {
			return;
		}

		var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.DryadsWard);
		dust.noGravity = true;
		dust.noLight = true;
	}

	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!IsActive || Main.rand.NextBool(2, 3)) {
			return;
		}

		target.AddBuff(BuffID.DryadsWardDebuff, 180);
	}
}

public class BarkBellowerGlobalNpc : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Dryad;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.Add<BarkBellower>(Condition.DownedEowOrBoc);
	}
}
