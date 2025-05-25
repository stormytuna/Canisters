using Canisters.Common;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.DataStructures;
using Terraria.Enums;

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
		return new Vector2(-6f, 0f);
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
	public override void SafeOnSpawn(Projectile projectile, IEntitySource source) {
		if (IsActive) {
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
