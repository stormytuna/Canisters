# High priority

## Balancing

Go through each canister and weapon, tweak balancing for:

- damage
- knockback
- explosion size
- damage, knockback, amount of child projectiles created where applicable

## Multiplayer compatibility

Go through basically everything, make sure netcode is correct

Might end up just adding Dave's easy packets mod for custom netcode

# Medium priority

## Visuals and effects

Go through each canister and weapon, tweak visuals:

- Add sounds
- Tweak visuals where they look a bit shit
- Add lighting and glowmasks
- Add some fancy pulsing for colours
- Revisit shooting out of muzzle, it's a bit imprecise

## Refactors

- Refactor `Player.PickAmmo` calls that `out _` to a new extension that does that for us
- Possibly refactor `CanisterUsingWeapon` to not *require* canister using weapons to have `_Canister` and `_Base`
  sprites

## Item stuff

- Make sure recipes make sense
- Make sure prices make sense
- May have made some weapons with incorrect width/heights :3
- Make sure using correct helper method - `Item.(buy|sell)Price` are NPC focused, should be using `Item.buyPrice` where an item is **not** bought
- Come up with some way to say whether a weapon launches or depletes - probably just implement `ModifyTooltips` in `CanisterUsingWeapon`
- Go through each items tooltip, make sure they make sense

# Low priority

## Achievements

There's a mod for this, would require mod calls or weak refs probably

## Config

Add a config option to reduce grinding

## Internal stuff

Take another look at documentation and internal names/organisation

## Empty asset

Refactor anything that has an empty asset to use the existing empty asset

## Unused localisation

Remove any unused localisation