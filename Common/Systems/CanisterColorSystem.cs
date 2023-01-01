using Canisters.Content.Items.Canisters;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Canisters.Common.Systems {
    public class CanisterColorSystem : ModSystem {
        public static Color Volatile {
            get {
                return new Color(45, 144, 255, 255);
            }
        }

        public static Color Verdant {
            get {
                return Color.Green;
            }
        }

        public static Color Glistening {
            get {
                return Color.Yellow;
            }
        }

        public static Color Blighted {
            get {
                return Color.Lime;
            }
        }

        public static Color GetCanisterColor(int canisterItemId) {
            if (canisterItemId == ModContent.ItemType<VolatileCanister>()) {
                return Volatile;
            }

            if (canisterItemId == ModContent.ItemType<VerdantCanister>()) {
                return Verdant;
            }

            if (canisterItemId == ModContent.ItemType<GlisteningCanister>()) {
                return Glistening;
            }

            if (canisterItemId == ModContent.ItemType<BlightedCanister>()) {
                return Blighted;
            }

            // Should never be hit, but compiler shouts at us without it
            return Color.White;
        }
    }
}