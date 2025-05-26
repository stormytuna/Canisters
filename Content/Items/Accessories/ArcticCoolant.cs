using Canisters.Content.Projectiles;
using Canisters.Helpers;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Accessories;

public class ArcticCoolant : ModItem
{
	public override void SetDefaults() {
		Item.width = 30;
		Item.height = 30;
		Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 4));
		Item.accessory = true;
	}

	public override void UpdateAccessory(Player player, bool hideVisual) {
		player.GetModPlayer<ArcticCoolantPlayer>().Active = true;
		player.GetModPlayer<ArcticCoolantPlayer>().Item = Item;
	}
}

public class ArcticCoolantPlayer : ModPlayer
{
	public bool Active;
	public Item Item;

	public override void ResetEffects() {
		Active = false;
	}

	public override void Load() {
		BaseFiredCanisterProjectile.OnExplode += OnExplode;
	}

	private static void OnExplode(Player player, Projectile projectile) {
		if (player.GetModPlayer<ArcticCoolantPlayer>().Active) {
			IEntitySource source = player.GetSource_Accessory(player.GetModPlayer<ArcticCoolantPlayer>().Item);
			Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f);
			Projectile.NewProjectile(source, projectile.Center, velocity, ModContent.ProjectileType<ArcticCoolantSnowflake>(), projectile.damage / 6, 0f);
		}
	}
}

public class ArcticCoolantDropCondition : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Flocko;
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArcticCoolant>(), 160));
	}
}

public class ArcticCoolantSnowflake : ModProjectile
{
	private NPC _target;
	private int _timer;
	private bool _firstFrame = true;

	public override void SetStaticDefaults() {
		ProjectileID.Sets.CultistIsResistantTo[Type] = true;
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 3;
	}

	public override void SetDefaults() {
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 2 * 60;

		Projectile.Opacity = 0.8f;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			Projectile.friendly = false;
			Projectile.scale = Main.rand.NextFloat(0.8f, 1f);
		}

		_timer++;

		if (_timer >= 18) {
			Projectile.friendly = true;
			if (_target is null || !_target.active) {
				System.Collections.Generic.List<NPC> nearbyNPCs = NpcHelpers.FindNearbyNPCs(32f * 16f, Projectile.Center, true);
				if (nearbyNPCs.Count > 0) {
					_target = Main.rand.Next(nearbyNPCs);
				}
				else {
					_target = null;
				}
			}
		}

		Projectile.rotation += 0.015f * Projectile.velocity.Length();

		if (_target is null) {
			Projectile.velocity *= 0.97f;
			return;
		}

		EntityHelpers.SmoothHoming(Projectile, _target.Center, 0.3f, 16f, _target.velocity, false);
		Projectile.timeLeft++;
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.SnowflakeIce, 5, 1f, 3f, noGravity: true);
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X != oldVelocity.X) {
			Projectile.velocity.X = -oldVelocity.X;
		}

		if (Projectile.velocity.Y != oldVelocity.Y) {
			Projectile.velocity.Y = -oldVelocity.Y;
		}

		return false;
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = height = 20;
		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}

	public override Color? GetAlpha(Color lightColor) {
		return Color.White * Projectile.Opacity;
	}

	public override bool PreDraw(ref Color lightColor) {
		DrawData drawData = new() {
			texture = TextureAssets.Projectile[Type].Value,
			position = (Projectile.Center - Main.screenPosition).Floor(),
			sourceRect = TextureAssets.Projectile[Type].Value.Frame(),
			scale = new Vector2(Projectile.scale),
			color = Projectile.GetAlpha(lightColor),
			rotation = Projectile.rotation,
			origin = TextureAssets.Projectile[Type].Value.Size() / 2f,
		};

		for (int i = Projectile.oldPos.Length - 1; i >= 0; i--) {
			DrawData trailData = drawData with {
				position = (Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition).Floor(),
				color = drawData.color * 0.5f,
			};
			trailData.Draw(Main.spriteBatch);
		}

		drawData.Draw(Main.spriteBatch);

		return false;
	}
}
