using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Canisters.Content.Items.Weapons;

public class SlimeySlinger : CanisterUsingWeapon
{
    public override FiringType FiringType => FiringType.Launched;

    public override Vector2 MuzzleOffset => new(0f, 0f);

    public override void SetDefaults() {
        // Base stats
        Item.width = 50;
        Item.height = 22;
        Item.value = Item.sellPrice(gold: 4);
        Item.rare = ItemRarityID.Pink;

        // Use stats
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = 22;
        Item.useAnimation = 22;
        Item.autoReuse = true;
        Item.noMelee = true;
        Item.noUseGraphic = true;

        // Weapon stats
        Item.shoot = ModContent.ProjectileType<VolatileCanister>();
        Item.shootSpeed = 15f;
        Item.damage = 16;
        Item.knockBack = 3f;
        Item.DamageType = DamageClass.Ranged;
        Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
    }

    public override Vector2? HoldoutOffset() => new Vector2(0f, 0f);
}

public class SlimySlingerGlobalProjectile : ShotByWeaponGlobalProjectile<SlimeySlinger> 
{
    private int numBounces = 3;

    public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
        if (!ShouldApply) {
            return true;
        }

        numBounces--;
        if (numBounces <= 0) {
            return true;
        }

        if (projectile.velocity.X != oldVelocity.X) {
            projectile.velocity.X = oldVelocity.X * -0.99f;
        }

        if (projectile.velocity.Y != oldVelocity.Y) {
            projectile.velocity.Y = oldVelocity.Y * -0.99f;
        }

        return false;
    }
}

// TODO: GlobalNPC to actually aquire it
