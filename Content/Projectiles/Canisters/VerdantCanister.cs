using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.Canisters {
    public class VerdantCanister : ModProjectile {
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = 2;
            Projectile.alpha = 100;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        public override string Texture => "Canisters/Content/Items/Canisters/VerdantCanister";

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if (Projectile.alpha != 255) {
                Explode();
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (Projectile.alpha != 255) {
                Explode();
                return false;
            }

            return base.OnTileCollide(oldVelocity);
        }

        private void Explode() {
            SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

            List<float> startRots = Main.rand.NextSegmentedAngles(4, 0.5f);
            if (Main.myPlayer == Projectile.owner) {
                for (int i = 0; i < 4; i++) {
                    int numVines = Main.rand.Next(3, 6);
                    float vineRot = Main.rand.NextGaussian(0f, 0.15f);
                    float startRot = startRots[i];
                    Vector2 offset = Vector2.UnitY.RotatedBy(startRot) * -20f;
                    Projectile vine = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + offset, Vector2.Zero, ModContent.ProjectileType<VerdantVine>(), Projectile.damage, Projectile.knockBack / 3f, Projectile.owner, numVines, vineRot);
                    vine.rotation = startRot;
                }
            }

            // Leafy dust
            for (int i = 0; i < 80; i++) {
                // Our base dust properties
                Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(0.8f, 1.2f));
                dust.velocity = velocity;
                dust.noGravity = true;

                if (Main.rand.NextBool(3)) {
                    // 1/3 dust becomes medium dust
                    float sizeMult = Main.rand.NextFloat(1f, 1.5f);
                    dust.scale *= sizeMult;
                    dust.velocity /= sizeMult;
                } else if (Main.rand.NextBool(4)) {
                    // 1/4 of the rest become little grass that's gravity effected
                    dust.velocity.X /= 4f;
                    dust.velocity.Y = MathF.Abs(dust.velocity.Y) / -4f;
                    dust.noGravity = false;
                }
            }

            // Nature energy dust
            for (int i = 0; i < 15; i++) {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.8f, 1.5f));
                dust.velocity = velocity;
            }

            // TODO: leaf gore

            Projectile.TurnToExplosion(96, 96);
        }
    }

    // This projectile does funky stuff
    // It's essentially a projectile that will stick to the muzzle of the gun that fires it
    // It will fire 3 gas clouds throughout its lifetime  
    // Basically meant to emulate the 4 shots of flamethrower without changing our items
    public class VerdantCanister_Depleted : ModProjectile {
        public override void SetDefaults() {
            // Base stats
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;

            // Weapon stats
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();
        }

        private Vector2 ownerOffset;
        private Vector2 startVelocity;
        private int maxFireCounter;
        private int numFired;

        private ref Player Owner => ref Main.player[Projectile.owner];

        private ref float AI_FireCounter => ref Projectile.ai[0];

        public override void OnSpawn(IEntitySource source) {
            ownerOffset = Owner.Center - Projectile.Center;
            startVelocity = Projectile.velocity;

            var heldProj = Main.projectile[Owner.heldProj].ModProjectile as CanisterUsingHeldProjectile;
            Projectile.timeLeft = heldProj.UseTimeAfterBuffs;
            maxFireCounter = Projectile.timeLeft / 3;
        }

        public override void AI() {
            // Make sure our projectile stays on the end of the gun
            Projectile.Center = Owner.Center - ownerOffset;
            Projectile.velocity = Vector2.Zero;

            // Actually shoot the projectile
            if (AI_FireCounter <= 0 && Collision.CanHit(Owner.Center, 0, 0, Projectile.Center, 0, 0) && numFired < 3 && Main.myPlayer == Projectile.owner) {
                Vector2 velocity = startVelocity * Main.rand.NextFloat(0.95f, 1.05f);
                velocity = velocity.RotatedByRandom(0.1f);
                Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, velocity, ModContent.ProjectileType<VerdantGas>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
                AI_FireCounter = maxFireCounter;
                numFired++;

                // Make some dust
                for (int i = 0; i < 5; i++) {
                    if (Main.rand.NextBool()) {
                        Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 1f));
                        dust.velocity = startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f);
                    }
                }
            }

            // Make some small dust
            // Nature energy dust
            if (Main.rand.NextBool(2)) {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 0.8f));
                dust.velocity = startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f); ;
            }

            AI_FireCounter--;

            base.AI();
        }
    }
}