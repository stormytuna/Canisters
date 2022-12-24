using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

public static class CanisterHelpers {
    public static void TurnToExplosion(this Projectile proj, int width, int height) {
        proj.timeLeft = 3;
        proj.tileCollide = false;
        proj.velocity = Vector2.Zero;
        proj.alpha = 255;
        proj.Resize(width, height);
    }
}

public enum FiringType {
    Canister,
    Regular
}

/// <summary>
/// Handles canister items <para/>
/// Be sure to set these helper properties in SetDefaults hook <para/>
/// <list type="bullet">
///     <item><term>LaunchedProjectileType</term><description> The projectile type of this canisters launched canister</description></item>
///     <item><term>DepletedProjetileType</term><description> The projectile type of this canisters shot projectile</description></item>
///     <item><term>DamageWhenLaunched</term><description> The damage this canister deals when launched</description></item>
///     <item><term>DamageWhenDepleted</term><description> The damage this canister deals when depleted to shoot a projectile</description></item>
/// </list>
/// This class overrides these ModProjectile methods, so be sure to either call base or understand what each override does when overriding in your weapon
/// <list type="bullet">
///     <item><term>ModifyTooltips</term><description> Handles damage when launched and depleted tooltips</description></item>
///     <item><term>ModifyShootStats</term><description> Sets the damage to the correct value when this item is used</description></item>
/// </list>
/// </summary>
public abstract class CanisterItem : ModItem {
    /// <summary>The projectile type of this canisters launched canister</summary>
    public int LaunchedProjectileType { get; set; }

    /// <summary>The projectile type of this canisters shot projectile</summary>
    public int DepletedProjectileType { get; set; }

    /// <summary>The damage this canister deals when launched</summary>
    private int _damageWhenLaunched;
    public int DamageWhenLaunched {
        get {
            return (int)Main.LocalPlayer.GetDamage(DamageClass.Ranged).ApplyTo(_damageWhenLaunched);
        }
        set {
            _damageWhenLaunched = value;
        }
    }

    /// <summary>The damage this canister deals when depleted to shoot a projectile</summary>
    private int _damageWhenDepleted;
    public int DamageWhenDepleted {
        get {
            return (int)Main.LocalPlayer.GetDamage(DamageClass.Ranged).ApplyTo(_damageWhenDepleted);
        }
        set {
            _damageWhenDepleted = value;
        }
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        var damageTip = tooltips.Find((tip) => tip.Name == "Damage");
        damageTip.Text = $"{DamageWhenLaunched} when launched";
        int index = tooltips.IndexOf(damageTip) + 1;
        tooltips.Insert(index, new(Mod, "DamageWhenDepleted", $"{DamageWhenDepleted} when depleted"));

        // TODO: Make these tooltips have the same colour, ie coloured damage types support

        base.ModifyTooltips(tooltips);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
        damage = DamageWhenLaunched;

        base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
    }
}

/// <summary>
/// Handles projectile held weapons for us <para/>
/// Be sure to set these helper properties in SetDefaults hook <para/>
/// <list type="bullet">
///     <item><term>HoldOutOffset</term><description> How far away the projectile will display from your character</description></item>
///     <item><term>FiringType</term><description> The firing type, either Canister or Regular</description></item>
///     <item><term>RotationOffset</term><description> How many extra radians this projectile will rotate when it's pointed to the mouse</description></item>
///     <item><term>CanisterOnSpriteFilepath</term><description> The filepath of the on sprite canister</description></item>
/// </list>
/// This class overrides these ModProjectile methods, so be sure to either call base or understand what each override does when overriding in your weapon
/// <list type="bullet">
///     <item><term>AI</term><description> Handles projectile direction, location and rotation and sets some values on the player</description></item>
///     <item><term>PreDraw</term><description> Handles drawing the canister colour</description></item>
///     <item><term>SendExtraAI and ReceiveExtraAI</term><description> Handles sending and receiving AI fields created by this class</description></item>
/// </list>
/// This class provides these virtual methods
/// <list type="bullet">
///     <item><term>Shoot</term><description> This is called each time the projectile shoots</description></item>
/// </list>
/// </summary>
public abstract class CanisterHeldProjectile : ModProjectile {
    /// <summary>Returns Main.player[Projectile.owner]</summary>
    public Player Owner => Main.player[Projectile.owner];

    /// <summary>How far away this projectile will appear from the player</summary>
    public float HoldOutOffset { get; set; }

    /// <summary>The firing type this weapon uses, either Canister or Regular</summary>
    public FiringType CanisterFiringType { get; set; }

    /// <summary>How much this projectile should be rotated when it points to the mouse</summary>
    public float RotationOffset { get; set; }

    /// <summary>The filepath of the canister on sprite texture this weapon should use. Set this to an empty string if the weapon doesn't have any canister on sprite</summary>
    public string CanisterOnSpriteFilepath { get; set; }

    /// <summary>This property acts as a frame counter</summary>
    public int AI_FrameCount { get; set; } = 0;

    // Helper property that applies attack speed for us
    private int UseTimeAfterBuffs => (int)((float)Owner.HeldItem.useTime * Owner.GetWeaponAttackSpeed(Owner.HeldItem));

    public virtual void Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }

    public override void AI() {
        // Just a var we use a couple times
        Vector2 toMouse = Main.MouseWorld - Projectile.Center;
        toMouse.Normalize();

        // Kill the projectile if we stop using it or can't use it
        if (!Owner.channel || Owner.CCed || !Owner.PickAmmo(Owner.HeldItem, out _, out _, out _, out _, out _, true)) {
            Projectile.Kill();
        }

        // Calls our Shoot override if we should
        int shootFrame = UseTimeAfterBuffs - 1;
        if (AI_FrameCount % shootFrame == 0) {
            Owner.PickAmmo(Owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockback, out int usedAmmoItemId);
            var canisterItem = ContentSamples.ItemsByType[usedAmmoItemId].ModItem as CanisterItem;
            if (CanisterFiringType == FiringType.Canister) {
                projToShoot = canisterItem.LaunchedProjectileType;
                damage += canisterItem.DamageWhenLaunched;
            } else {
                projToShoot = canisterItem.DepletedProjectileType;
                damage += canisterItem.DamageWhenLaunched;
            }
            EntitySource_ItemUse_WithAmmo source = new(Owner, Owner.HeldItem, usedAmmoItemId);
            Vector2 velocity = toMouse * speed;

            Shoot(Owner, source, Owner.MountedCenter, velocity, projToShoot, damage, knockback);
        }

        // Set direction and rotation
        Projectile.direction = 1;
        if (Math.Sign(Main.MouseWorld.X - Owner.Center.X) == -1)
            Projectile.direction = -1;
        Projectile.rotation = toMouse.ToRotation() - Projectile.direction * RotationOffset + MathHelper.PiOver2;
        Projectile.spriteDirection = Projectile.direction;

        // Set position and velocity
        Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + toMouse * HoldOutOffset;
        Projectile.velocity = Vector2.Zero;

        // Set timeleft
        Projectile.timeLeft = 2;

        // Set some values on our player
        Owner.ChangeDir(Projectile.direction);
        Owner.heldProj = Projectile.whoAmI;
        Owner.SetDummyItemTime(2);
        Owner.itemRotation = Projectile.DirectionFrom(Owner.MountedCenter).ToRotation();
        if (Projectile.Center.X < Owner.MountedCenter.X) {
            Owner.itemRotation += (float)Math.PI;
        }
        Owner.itemRotation = MathHelper.WrapAngle(Owner.itemRotation);

        // Increment our frame count
        AI_FrameCount++;

        base.AI();
    }

    private Texture2D _canisterOnSprite;
    private Texture2D CanisterOnSprite {
        get {
            if (_canisterOnSprite is null) {
                _canisterOnSprite = ModContent.Request<Texture2D>(CanisterOnSpriteFilepath).Value;
            }

            return _canisterOnSprite;
        }
    }

    public override bool PreDraw(ref Color lightColor) {
        if (CanisterOnSpriteFilepath == "") {
            return base.PreDraw(ref lightColor);
        }

        // TODO: Draw canister

        return base.PreDraw(ref lightColor);
    }

    // Sends and receives our ai fields, not even sure if we need this but w/e
    public override void SendExtraAI(BinaryWriter writer) {
        writer.Write(AI_FrameCount);

        base.SendExtraAI(writer);
    }
    public override void ReceiveExtraAI(BinaryReader reader) {
        AI_FrameCount = reader.ReadInt32();

        base.ReceiveExtraAI(reader);
    }
}