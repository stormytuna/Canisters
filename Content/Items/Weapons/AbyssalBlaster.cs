using Canisters.DataStructures;
using MonoMod.Cil;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class AbyssalBlaster : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(40f, 0f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-4f, 2f);
	}

	public override void Load() {
		IL_Item.GetShimmered += InsertAbyssalBlasterShimmerCondition;
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(50, 50, 10f, 36, 4f);
		Item.width = 56;
		Item.height = 18;
		Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 7));
		// TODO: revisit sound
		Item.UseSound = SoundID.Item10 with { PitchRange = (-1f, -0.8f) };
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.ProjectileCount *= 3;
		stats.TotalSpread += 0.2f;
	}

	private void InsertAbyssalBlasterShimmerCondition(ILContext il) {
		ILCursor cursor = new(il);

		// Find label for end of if else chain
		ILLabel endIfElseChainLabel = cursor.DefineLabel();
		cursor.GotoNext(
			MoveType.Before,
			i => i.MatchBr(out endIfElseChainLabel),
			i => i.MatchLdloc0(),
			i => i.MatchLdcI4(1326)
		);

		cursor.Index = 0;

		// Insert before if statement begins
		cursor.GotoNext(MoveType.Before, i => i.MatchLdsfld<ItemID.Sets>(nameof(ItemID.Sets.CoinLuckValue)));

		cursor.EmitLdloc0(); // shimmerEquivalentType
		cursor.EmitLdarg0(); // this (the item)

		cursor.EmitDelegate((int shimmerEquivalentType, Item item) => {
			if (shimmerEquivalentType == ModContent.ItemType<InfernalCannon>() && NPC.downedPlantBoss) {
				int originalStack = item.stack;
				item.SetDefaults(ModContent.ItemType<AbyssalBlaster>());
				item.stack = originalStack;
				item.shimmered = true;
				return true;
			}

			return false;
		});

		cursor.EmitBrtrue(endIfElseChainLabel);
	}
}
