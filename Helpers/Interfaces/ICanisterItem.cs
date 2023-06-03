namespace Canisters.Helpers.Interfaces;

/// <summary>
///     Handles canister items
///     <para />
///     Be sure to set these helper properties in SetDefaults hook
///     <para />
///     <list type="bullet">
///         <item>
///             <term>LaunchedProjectileType</term><description> The projectile type of this canisters launched canister</description>
///         </item>
///         <item>
///             <term>DepletedProjetileType</term><description> The projectile type of this canisters shot projectile</description>
///         </item>
///     </list>
/// </summary>
public interface ICanisterItem
{
	/// <summary>The projectile type of this canisters launched canister</summary>
	public int LaunchedProjectileType => -1;

	/// <summary>The projectile type of this canisters shot projectile</summary>
	public int DepletedProjectileType => -1;
}