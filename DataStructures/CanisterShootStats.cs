using Canisters.Helpers.Enums;

namespace Canisters.DataStructures;

public record struct CanisterShootStats
{
	public CanisterFiringType FiringType;
	public Vector2 Velocity;
	public Vector2 Position;
	public int ProjectileType;
	public int Damage;
	public float Knockback;
	public int ProjectileCount;
	public float TotalSpread;
	
	public bool IsDepleted => FiringType == CanisterFiringType.Depleted;
	public bool IsLaunched => FiringType == CanisterFiringType.Launched;
}