using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Canisters.Content.Projectiles.VolatileCanister;
using Terraria.DataStructures;

namespace Canisters.Content.Items.Weapons;
public class Pneumaticannon : CanisterUsingWeapon
{
    public override FiringType FiringType => FiringType.Launched;

    public override Vector2 MuzzleOffset => new(22f, -2f);

    public override void SetDefaults() {
        // Base stats
        Item.width = 50;
        Item.height = 22;
        Item.value = Item.sellPrice(gold: 4);
        Item.rare = ItemRarityID.LightRed;

        // Use stats
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = 32;
        Item.useAnimation = 32;
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

    public override Vector2? HoldoutOffset() => new Vector2(-6f,0f);

    public override void ShootProjectile(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int owner) {
        Projectile canister = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, owner);
        canister.extraUpdates = 2; // TODO: Wont work in MP!!
    }
}

// TODO: Make it actually sold by the girl!!