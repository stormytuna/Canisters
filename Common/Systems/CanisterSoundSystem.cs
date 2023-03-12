using Canisters.Content.Items.Canisters;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Common.Systems
{
    public class CanisterSoundSystem : ModSystem
    {
        public static SoundStyle GetDepletedCanisterSound(int canisterItemId) {
            if (canisterItemId == ModContent.ItemType<VolatileCanister>()) {
                return SoundID.SplashWeak with {
                    MaxInstances = 0,
                    Volume = 0.7f,
                    PitchRange = (-0.6f, -0.4f)
                };
            }

            if (canisterItemId == ModContent.ItemType<VerdantCanister>()) {
                return SoundID.Item34 with {
                    MaxInstances = 0,
                    Volume = 0.5f,
                    PitchRange = (-0.1f, 0.1f)
                };
            }

            if (canisterItemId == ModContent.ItemType<GlisteningCanister>()) {
                return SoundID.Item20 with {
                    MaxInstances = 0,
                    Volume = 0.8f,
                    PitchRange = (0.7f, 0.9f)
                };
            }

            if (canisterItemId == ModContent.ItemType<BlightedCanister>()) {
                return SoundID.Item94 with {
                    MaxInstances = 0,
                    Volume = 0.4f,
                    Pitch = 1.35f,
                    PitchVariance = 0.4f
                };
            }

            if (canisterItemId == ModContent.ItemType<HarmonicCanister>()) {
                return SoundID.Item42 with {
                    MaxInstances = 0,
                    Volume = 0.4f,
                    PitchRange = (0.1f, 0.5f)
                };
            }

            return SoundID.Item1;
        }
    }
}
