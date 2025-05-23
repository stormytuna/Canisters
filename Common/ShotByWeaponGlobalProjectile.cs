using System.IO;
using Canisters.Content.Items.Weapons;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Canisters.Common;

public abstract class ShotByWeaponGlobalProjectile<TWeapon> : GlobalProjectile
	where TWeapon : BaseCanisterUsingWeapon
{
	protected int ShotByCanisterType { get; private set; }
	
	protected bool IsActive { get; private set; }

	public override bool InstancePerEntity {
		get => true;
	}

	public virtual bool ApplyFromParent() {
		return false;
	}

	public virtual void SafeOnSpawn(Projectile projectile, IEntitySource source) { }

	public sealed override void OnSpawn(Projectile projectile, IEntitySource source) {
		bool shotByTWeapon = false;
		if (source is EntitySource_ItemUse_WithAmmo { Item.ModItem: TWeapon } itemUseSource) {
			shotByTWeapon = true;
			ShotByCanisterType = itemUseSource.AmmoItemIdUsed;
		}

		bool appliesToParent = false;
		if (source is EntitySource_Parent { Entity: Projectile parentProjectile }) {
			appliesToParent = parentProjectile.GetGlobalProjectile(this).IsActive;
			ShotByCanisterType = parentProjectile.GetGlobalProjectile(this).ShotByCanisterType;
		}
		
		IsActive = shotByTWeapon || (appliesToParent && ApplyFromParent());

		SafeOnSpawn(projectile, source);
	}

	// Syncing extraUpdates as it's set by a couple of our weapons using this class
	public sealed override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter) {
		bitWriter.WriteBit(IsActive);
		binaryWriter.Write7BitEncodedInt(ShotByCanisterType);
		binaryWriter.Write7BitEncodedInt(projectile.extraUpdates);
	}

	public sealed override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader) {
		IsActive = bitReader.ReadBit();
		ShotByCanisterType = binaryReader.Read7BitEncodedInt();
		projectile.extraUpdates = binaryReader.Read7BitEncodedInt();
	}
}
