using System.IO;
using Canisters.Common;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader.IO;

namespace Canisters.Content.Items.Weapons;

public class Pneumaticannon : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(22f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-6f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(32, 32, 10f, 16, 3f);
		Item.width = 48;
		Item.height = 14;
		Item.SetShopValues(ItemRarityColor.Pink5, Item.sellPrice(gold: 75));
	}
}

public class PneumaticannonGlobalItem : ShotByWeaponGlobalProjectile<Pneumaticannon>
{
	public override void SafeOnSpawn(Projectile projectile, IEntitySource source) {
		if (IsActive) {
			projectile.extraUpdates = 2;
		}
	}

	public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter) {
		binaryWriter.Write7BitEncodedInt(projectile.extraUpdates);
	}

	public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader) {
		projectile.extraUpdates = binaryReader.Read7BitEncodedInt();
	}
}

public class PneumaticannonGlobalNpc : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Steampunker;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.Add<Pneumaticannon>();
	}
}
