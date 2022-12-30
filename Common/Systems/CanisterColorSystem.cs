using Canisters.Content.Items.Canisters;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Canisters.Common.Systems {
    public class CanisterColorSystem : ModSystem {
        public static Color Volatile {
            get {
                return Color.Blue;
            }
        }

        public static Color Verdant {
            get {
                return Color.Green;
            }
        }

        public static Color GetCanisterColor(int canisterItemId) {
            if (canisterItemId == ModContent.ItemType<VolatileCanister>()) {
                return Volatile;
            }

            if (canisterItemId == ModContent.ItemType<VerdantCanister>()) {
                return Verdant;
            }

            // Should never be hit, but compiler shouts at us without it
            return Color.White;
        }
    }
}