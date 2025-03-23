using System.Linq;
using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Weapons;

public class GraniteCannon : CanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(36f, 0f);
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 44;
		Item.height = 16;
		Item.value = Item.buyPrice(silver: 30);
		Item.rare = ItemRarityID.Blue;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 42;
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
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-2f, 0f);
	}

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
		ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.16f);
	}
}

public class GraniteCannonGlobalNpc : GlobalNPC
{
	private static readonly int[] _graniteEnemies = { NPCID.GraniteFlyer, NPCID.GraniteGolem };

	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return _graniteEnemies.Contains(entity.type);
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraniteCannon>(), 20));
	}
}
