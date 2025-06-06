using Canisters.Common;
using Canisters.DataStructures;
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
		return new Vector2(-4f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(36, 36, 12f, 45, 8f);
		Item.width = 54;
		Item.height = 16;
		Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(silver: 50));
		Item.UseSound = SoundID.Item10 with { PitchRange = (-1f, -0.8f) };

		Item.crit = 2;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.HellstoneBar, 12)
			.AddIngredient(ItemID.IllegalGunParts)
			.AddTile(TileID.Hellforge)
			.Register();
	}
}

public class InfernalCannonGlobalProjectile : ShotByWeaponGlobalProjectile<InfernalCannon>
{
	public override bool ApplyFromParent() {
		return true;
	}

	public override void AI(Projectile projectile) {
		if (!IsActive || projectile.hide || Main.rand.NextBool(4, 5)) {
			return;
		}

		Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.ShadowbeamStaff);
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
