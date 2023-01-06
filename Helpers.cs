using Canisters.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

public static class Helpers
{
    /// <summary>
    ///     Turns the projectile into an explosion
    /// </summary>
    /// <param name="width">The width of the explosion</param>
    /// <param name="height">The height of the explosion</param>
    public static void TurnToExplosion(this Projectile proj, int width, int height) {
        proj.velocity = new Vector2(0f, 0f);
        proj.timeLeft = 3;
        proj.penetrate = -1;
        proj.tileCollide = false;
        proj.alpha = 255;
        proj.Resize(width, height);
        // TODO: implement this
        //proj.knockback = /* param */ knockback;
    }

    // Adapted from here https://bitbucket.org/Superbest/superbest-random/src/f067e1dc014c31be62c5280ee16544381e04e303/Superbest%20random/RandomExtensions.cs#lines-19
    /// <summary>
    ///     Generates normally distributed numbers. Each operation makes two Gaussians for the price of one, and apparently they can be cached or something for better performance, but who cares.
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
    ///     Generates a list of angles generated within segments of a circle
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
    ///     Generates a random float between 0 and two pi
    /// </summary>
    public static float NextRadian(this UnifiedRandom rand) {
        return rand.NextFloat(MathHelper.TwoPi);
    }

    /// <summary>
    ///     An enumerable of all nearby npcs
    /// </summary>
    public static IEnumerable<NPC> FindNearbyNPCs(float range, Vector2 worldPos) {
        return Main.npc.SkipLast(1).Where((npc) => npc.DistanceSQ(worldPos) < range * range && npc.active && !npc.CountsAsACritter && !npc.friendly && !npc.dontTakeDamage && !npc.immortal);
    }

    /// <summary>
    ///     Ease in interpolation between the start and end
    /// </summary>
    /// <param name="start">The starting value, will return this when amount == 0</param>
    /// <param name="end">The ending value, will return this when amount == 1</param>
    /// <param name="amount">The amount to lerp by</param>
    /// <param name="exponent">The exponent of the easing curve to use, larger values cause more easing</param>
    /// <returns>Returns the ease in interpolation between start and end</returns>
    public static float EaseIn(float start, float end, float amount, int exponent) {
        if (amount <= 0f) {
            return start;
        }
        if (amount >= 1f) {
            return end;
        }

        float amountExp = MathF.Pow(amount, exponent);
        return MathHelper.Lerp(start, end, amountExp);
    }

    public static void MakeDebugDust(Vector2 position, Color color) {
        Dust d = Dust.NewDustPerfect(position, 303, newColor: color);
        d.velocity = Vector2.Zero;
        d.noGravity = true;
    }
}

public static class LightningHelper
{
    public static void MakeDust(Vector2 source, Vector2 dest, int dustId, float scale, float sway = 80f, float jagednessNumerator = 1f) {
        var dustPoints = CreateBolt(source, dest, sway, jagednessNumerator);
        foreach (var point in dustPoints) {
            Dust d = Dust.NewDustPerfect(point, dustId, Scale: scale);
            d.noGravity = true;
            d.velocity = Vector2.Zero;
        }
    }

    public static List<Vector2> CreateBolt(Vector2 source, Vector2 dest, float sway = 80f, float jagednessNumerator = 1f) {
        var results = new List<Vector2>();
        Vector2 tangent = dest - source;
        Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
        float length = tangent.Length();

        List<float> positions = new List<float> {
            0
        };

        for (int i = 0; i < length; i++)
            positions.Add(Main.rand.NextFloat(0f, 1f));

        positions.Sort();

        float jaggedness = jagednessNumerator / sway;

        Vector2 prevPoint = source;
        float prevDisplacement = 0f;
        for (int i = 1; i < positions.Count; i++) {
            float pos = positions[i];

            // used to prevent sharp angles by ensuring very close positions also have small perpendicular variation.
            float scale = (length * jaggedness) * (pos - positions[i - 1]);

            // defines an envelope. Points near the middle of the bolt can be further from the central line.
            float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

            float displacement = Main.rand.NextFloat(-sway, sway);
            displacement -= (displacement - prevDisplacement) * (1 - scale);
            displacement *= envelope;

            Vector2 point = source + pos * tangent + displacement * normal;
            results.Add(point);
            prevPoint = point;
            prevDisplacement = displacement;
        }

        results.Add(prevPoint);

        return results;
    }
}

public enum FiringType
{
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
public interface ICanisterItem
{
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
public abstract class CanisterUsingWeapon : ModItem
{
    private Asset<Texture2D> _baseTexture;
    private Asset<Texture2D> BaseTexture { get => _baseTexture ??= ModContent.Request<Texture2D>(Texture + "_Base"); }

    private Asset<Texture2D> _canisterTexture;
    private Asset<Texture2D> CanisterTexture { get => _canisterTexture ??= ModContent.Request<Texture2D>(Texture + "_Canister"); }

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
public abstract class CanisterUsingHeldProjectile : ModProjectile
{
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

    private int AI_Lifetime { get; set; }

    // Helper property that applies attack speed for us
    public int UseTimeAfterBuffs => (int)((float)Owner.HeldItem.useTime * CombinedHooks.TotalUseTimeMultiplier(Owner, Owner.HeldItem));

    public virtual void Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }

    public override void AI() {
        Vector2 toMouse = Main.MouseWorld - Projectile.Center;
        toMouse.Normalize();
        bool hasAmmo = Owner.PickAmmo(Owner.HeldItem, out _, out _, out _, out _, out _, true);
        bool doShoot = false; // Used so we can delay the calling of our actual shoot hook since we need rotation set early

        // Kill the projectile if we stop using it or can't use it
        if (((!Owner.channel || !hasAmmo) && AI_Lifetime <= 1) || Owner.CCed) {
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
            var canisterItem = ContentSamples.ItemsByType[usedAmmoItemId].ModItem as ICanisterItem;
            if (CanisterFiringType == FiringType.Canister) {
                projToShoot = canisterItem.LaunchedProjectileType;
            } else {
                projToShoot = canisterItem.DepletedProjectileType;
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

            AI_Lifetime = UseTimeAfterBuffs + 1;
            Projectile.netUpdate = true;
        }

        // Set timeleft
        Projectile.timeLeft = 2;

        AI_FrameCount++;
        AI_Lifetime--;

        base.AI();
    }

    private Asset<Texture2D> _baseTexture;
    private Asset<Texture2D> BaseTexture { get => _baseTexture ??= ModContent.Request<Texture2D>(Texture + "_Base"); }

    private Asset<Texture2D> _canisterTexture;
    private Asset<Texture2D> CanisterTexture { get => _canisterTexture ??= ModContent.Request<Texture2D>(Texture + "_Canister"); }

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
        writer.Write(AI_Lifetime);

        base.SendExtraAI(writer);
    }
    public override void ReceiveExtraAI(BinaryReader reader) {
        AI_FrameCount = reader.ReadInt32();
        AI_Lifetime = reader.ReadInt32();

        base.ReceiveExtraAI(reader);
    }
}