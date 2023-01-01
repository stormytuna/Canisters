using Canisters.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

public static class CanisterHelpers {
    public static void TurnToExplosion(this Projectile proj, int width, int height) {
        proj.timeLeft = 3;
        proj.tileCollide = false;
        proj.velocity = Vector2.Zero;
        proj.alpha = 255;
        proj.Resize(width, height);
    }

    // Adapted from here https://bitbucket.org/Superbest/superbest-random/src/f067e1dc014c31be62c5280ee16544381e04e303/Superbest%20random/RandomExtensions.cs#lines-19
    /// <summary>
    /// Generates normally distributed numbers. Each operation makes two Gaussians for the price of one, and apparently they can be cached or something for better performance, but who cares.
    /// </summary>
    /// <param name="r"></param>
    /// <param name = "mu">Mean of the distribution</param>
    /// <param name = "sigma">Standard deviation</param>
    /// <returns></returns>
    public static float NextGaussian(this UnifiedRandom r, float mu = 0, float sigma = 1) {
        float u1 = r.NextFloat();
        float u2 = r.NextFloat();

        float stdNormal = MathF.Sqrt(-2f * MathF.Log(u1)) * MathF.Sin(2f * MathHelper.Pi * u2);

        float normal = mu + sigma * stdNormal;

        return normal;
    }

    /// <summary>
    /// Generates a list of angles generated within segments of a circle
    /// </summary>
    /// <param name="rand">The UnifiedRandom to use</param>
    /// <param name="numSegments">The number of segments, or the number of angles to generate</param>
    /// <param name="overlap">The amount of overlap between each segment. Note that this is the total overlap, not the overlap on each side of the segment</param>
    /// <param name="randomOffset">Whether or not a random offset will be added to the angles. If set to false, no offset is added</param>
    /// <returns></returns>
    public static List<float> NextSegmentedAngles(this UnifiedRandom rand, int numSegments, float overlap = 0f, bool randomOffset = true) {
        List<float> angles = new();

        // Build our list
        float offset = randomOffset ? rand.NextFloat(MathHelper.TwoPi) : 0f;
        for (int i = 0; i < numSegments; i++) {
            float angle = (((float)i / (float)numSegments) * MathHelper.TwoPi) + offset;
            angles.Add(angle);
        }

        // Randomly rotate our angles
        for (int i = 0; i < angles.Count; i++) {
            float rotationMax = (MathHelper.TwoPi / (float)numSegments) + overlap;
            float rotation = rand.NextFloat(-rotationMax / 2f, rotationMax / 2f);
            angles[i] += rotation;
        }

        return angles;
    }

    /// <summary>
    /// Generates a random float between 0 and two pi
    /// </summary>
    public static float NextRadian(this UnifiedRandom rand) {
        return rand.NextFloat(MathHelper.TwoPi);
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
/// </list>
/// </summary>
public interface ICanisterItem {
    /// <summary>The projectile type of this canisters launched canister</summary>
    public int LaunchedProjectileType { get => -1; }

    /// <summary>The projectile type of this canisters shot projectile</summary>
    public int DepletedProjectileType { get => -1; }
}

/// <summary>
/// Handles weapons that visually show the canister <para/>
/// This class overrides these ModItem methods, so be sure to either call base or understand what each override does when overriding in your weapon
/// <list type="bullet">
///     <item><term>CanConsumeAmmo</term><description> Prevents the item from consuming ammo itself so only our held projectile will consume ammo</description></item>
///     <item><term>PreDrawInInventory</term><description> Draws the item with the canister coloured based on what canister the player will fire</description></item>
/// </list>
/// </summary>
public abstract class CanisterUsingWeapon : ModItem {
    private Asset<Texture2D> _baseTexture;
    private Asset<Texture2D> BaseTexture {
        get {
            _baseTexture ??= ModContent.Request<Texture2D>(Texture + "_Base");
            return _baseTexture;
        }
    }

    private Asset<Texture2D> _canisterTexture;
    private Asset<Texture2D> CanisterTexture {
        get {
            _canisterTexture ??= ModContent.Request<Texture2D>(Texture + "_Canister");
            return _canisterTexture;
        }
    }

    public override bool CanConsumeAmmo(Item ammo, Player player) => player.heldProj != -1;

    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
        // Check if we have any canisters
        var player = Main.LocalPlayer;
        if (player.PickAmmo(Item, out _, out _, out _, out _, out int usedAmmoItemId, true)) {
            // Draw the weapon base
            spriteBatch.Draw(BaseTexture.Value, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);

            // Draw the canister
            Color canisterColor = CanisterColorSystem.GetCanisterColor(usedAmmoItemId);
            spriteBatch.Draw(CanisterTexture.Value, position, frame, canisterColor, 0f, origin, scale, SpriteEffects.None, 0);

            return false;
        }

        return true;
    }
}

/// <summary>
/// Handles projectile held weapons for us <para/>
/// Be sure to set these helper properties in SetDefaults hook <para/>
/// <list type="bullet">
///     <item><term>HoldOutOffset</term><description> How far away the projectile will display from your character</description></item>
///     <item><term>CanisterFiringType</term><description> The firing type, either Canister or Regular</description></item>
///     <item><term>RotationOffset</term><description> How many extra radians this projectile will rotate when it's pointed to the mouse</description></item>
///     <item><term>Texture</term><description> This is actually provided by ModProjectile, but make sure to override it and set it to the items sprite filepath</description></item>
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
public abstract class CanisterUsingHeldProjectile : ModProjectile {
    /// <summary>Returns Main.player[Projectile.owner]</summary>
    public Player Owner => Main.player[Projectile.owner];

    /// <summary>How far away this projectile will appear from the player</summary>
    public float HoldOutOffset { get; set; }

    /// <summary>The firing type this weapon uses, either Canister or Regular</summary>
    public FiringType CanisterFiringType { get; set; }

    /// <summary>How much this projectile should be rotated when it points to the mouse</summary>
    public float RotationOffset { get; set; }

    /// <summary>How far from the center the projectile will be fired from, assuming the projectile looks like its sprite (facing to the right)</summary>
    public Vector2 MuzzleOffset { get; set; }

    /// <summary>This property acts as a frame counter</summary>
    public int AI_FrameCount { get; set; } = 0;

    // Helper property that applies attack speed for us
    public int UseTimeAfterBuffs => (int)((float)Owner.HeldItem.useTime * Owner.GetWeaponAttackSpeed(Owner.HeldItem));

    public virtual void Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }

    public override void AI() {
        Vector2 toMouse = Main.MouseWorld - Projectile.Center;
        toMouse.Normalize();
        bool hasAmmo = Owner.PickAmmo(Owner.HeldItem, out _, out _, out _, out _, out _, true);
        bool doShoot = false; // Used so we can delay the calling of our actual shoot hook since we need rotation set early

        // Kill the projectile if we stop using it or can't use it
        if (((!Owner.channel || !hasAmmo) && Owner.ItemAnimationEndingOrEnded) || Owner.CCed) {
            Projectile.Kill();
            return;
        }

        // Set our rotation and doShoot if we are shooting this frame
        if (AI_FrameCount % UseTimeAfterBuffs == 0) {
            // Set rotation
            Projectile.rotation = toMouse.ToRotation() + RotationOffset;
            doShoot = true;
        }

        // Set some stuff based on our rotation
        Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + Projectile.rotation.ToRotationVector2() * HoldOutOffset;
        Projectile.velocity = Vector2.Zero;
        Projectile.direction = Math.Sign(Projectile.Center.X - Owner.Center.X);
        Projectile.spriteDirection = Projectile.direction;

        // Set some values on our player
        Owner.ChangeDir(Projectile.direction);
        Owner.heldProj = Projectile.whoAmI;
        Owner.itemRotation = Projectile.DirectionFrom(Owner.MountedCenter).ToRotation();
        if (Projectile.Center.X < Owner.MountedCenter.X) {
            Owner.itemRotation += (float)Math.PI;
        }
        Owner.itemRotation = MathHelper.WrapAngle(Owner.itemRotation);

        // Actually call our shoot hook
        if (doShoot) {
            Owner.PickAmmo(Owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockback, out int usedAmmoItemId);

            // Get our projectile type
            var canisterItem = ContentSamples.ItemsByType[usedAmmoItemId].ModItem as CanisterItem;
            if (CanisterFiringType == FiringType.Canister) {
                projToShoot = canisterItem.LaunchedProjectileType;
                damage += canisterItem.DamageWhenLaunched;
            } else {
                projToShoot = canisterItem.DepletedProjectileType;
                damage += canisterItem.DamageWhenDepleted;
            }

            // Get some other params
            EntitySource_ItemUse_WithAmmo source = new(Owner, Owner.HeldItem, usedAmmoItemId);
            Vector2 velocity = toMouse * speed;
            Vector2 offset = new(MuzzleOffset.X, MuzzleOffset.Y * Projectile.direction);
            offset = offset.RotatedBy(Projectile.rotation);
            Vector2 position = Projectile.Center + offset;

            Shoot(Owner, source, position, velocity, projToShoot, damage, knockback);

            // This makes it so we keep our item used until the potential next shot
            Owner.SetDummyItemTime(UseTimeAfterBuffs + 1);
        }

        // Set timeleft
        Projectile.timeLeft = 2;

        // Increment our frame count
        AI_FrameCount++;

        base.AI();
    }

    private Asset<Texture2D> _baseTexture;
    private Asset<Texture2D> BaseTexture {
        get {
            _baseTexture ??= ModContent.Request<Texture2D>(Texture + "_Base");
            return _baseTexture;
        }
    }

    private Asset<Texture2D> _canisterTexture;
    private Asset<Texture2D> CanisterTexture {
        get {
            _canisterTexture ??= ModContent.Request<Texture2D>(Texture + "_Canister");
            return _canisterTexture;
        }
    }

    public override bool PreDraw(ref Color lightColor) {
        if (Owner.PickAmmo(Owner.HeldItem, out _, out _, out _, out _, out int usedAmmoItemID, true)) {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Rectangle frame = new(0, 0, BaseTexture.Width(), BaseTexture.Height());
            Color drawColor = lightColor;
            float rotation = Projectile.rotation;
            Vector2 origin = frame.Size() / 2f;
            float scale = Projectile.scale;
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            Main.EntitySpriteDraw(BaseTexture.Value, position, frame, drawColor, rotation, origin, scale, effects, 0);

            Point projectileTileCoords = Projectile.Center.ToTileCoordinates();
            Color canisterColor = CanisterColorSystem.GetCanisterColor(usedAmmoItemID) * Lighting.Brightness(projectileTileCoords.X, projectileTileCoords.Y);
            Main.EntitySpriteDraw(CanisterTexture.Value, position, frame, canisterColor, rotation, origin, scale, effects, 0);

            return false;
        }

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