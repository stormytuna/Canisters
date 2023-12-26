using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Canisters.Content.Projectiles.VolatileCanister;

namespace Canisters.Content.Items.Weapons;

public class Splattergun : CanisterUsingWeapon
{
    public override FiringType FiringType => FiringType.Depleted;

    public override Vector2 MuzzleOffset => new(26f, -2f);

    public override void SetDefaults() {
        // Base stats
        Item.width = 50;
        Item.height = 22;
        Item.value = Item.sellPrice(gold: 4);
        Item.rare = ItemRarityID.LightRed;

        // Use stats
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = 8;
        Item.useAnimation = 8;
        Item.autoReuse = true;
        Item.noMelee = true;
        Item.noUseGraphic = true;

        // Weapon stats
        Item.shoot = ModContent.ProjectileType<VolatileCanister>();
        Item.shootSpeed = 10f;
        Item.damage = 16;
        Item.knockBack = 3f;
        Item.DamageType = DamageClass.Ranged;
        Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
    }

    // TODO: Recipe

    public override Vector2? HoldoutOffset() => new Vector2(0f, 0f);

    public override void ApplyShootStats(ref Vector2 velocity, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread) {
        velocity *= 1.5f;
    }
}
