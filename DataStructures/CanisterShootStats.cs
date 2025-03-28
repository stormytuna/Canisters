using Canisters.Helpers.Enums;

namespace Canisters.DataStructures;

public record struct CanisterShootStats
{
	public int Damage;
	public CanisterFiringType FiringType;
	public float Knockback;
	public Vector2 Position;
	public int ProjectileCount;
	public int ProjectileType;
	public float TotalSpread;
	public Vector2 Velocity;

	public bool IsDepleted {
		get => FiringType == CanisterFiringType.Depleted;
	}

	public bool IsLaunched {
		get => FiringType == CanisterFiringType.Launched;
	}
}
