using Canisters.Common;
using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class InfernalCannon : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(36f, 0f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-8f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(36, 36, 13f, 42, 8f);
		Item.width = 54;
		Item.height = 16;
		Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(silver: 50));
	}
	
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.HellstoneBar, 16)
			.AddTile(TileID.Hellforge)
			.Register();
	}
}

public class InfernalCannonGlobalProjectile : ShotByWeaponGlobalProjectile<InfernalCannon>
{
	public override void AI(Projectile projectile) {
		if (!IsActive || projectile.hide || Main.rand.NextBool(4, 5)) {
			return;
		}

		var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.ShadowbeamStaff);
		dust.scale = Main.rand.NextFloat(1.5f, 2f);
		dust.noGravity = true;
		dust.noLight = true;
	}

	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!IsActive || Main.rand.NextBool(2, 3)) {
			return;
		}

		target.AddBuff(BuffID.ShadowFlame, 180);
	}
}
