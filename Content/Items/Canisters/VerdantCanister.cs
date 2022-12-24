using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters {
    public class VerdantCanister : CanisterItem {
        public override void SetStaticDefaults() {
            SacrificeTotal = 99;

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = 2;

            // Use stats
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;

            // Weapon stats
            Item.shoot = ModContent.ProjectileType<Projectiles.Canisters.VerdantCanister>();
            Item.shootSpeed = 8f;
            Item.damage = 1;
            Item.crit = 4;
            Item.knockBack = 6f;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = ModContent.ItemType<MagmaCanister>();

            // CanisterItem stats
            LaunchedProjectileType = Item.shoot;
            DepletedProjectileType = -1;
            DamageWhenLaunched = 15;
            DamageWhenDepleted = 2;

            base.SetDefaults();
        }
    }

    public class VerdantCanisterGlobalNPC : GlobalNPC {
        public override void SetupShop(int type, Chest shop, ref int nextSlot) {
            if (type != NPCID.Dryad) return;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<VerdantCanister>());
            nextSlot++;

            base.SetupShop(type, shop, ref nextSlot);
        }
    }
}
