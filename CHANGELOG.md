## Version 0.5.0
* Release for 2.0.0
* Increased compatability with other mods that add ModifierDescriptors

## Version 0.4.5
* Release for 1.4.0
* IInitiatorDemoralizeHandler added.

## Version 0.4.4

* IStatBonusCalculatedHandler handles more component types.

## Version 0.4.3
This is a breaking patch, you will may need to recompile.

* Added support for effect disjoint.
* Added support for scroll creation.
* Support for modded selections to show up correctly in feat tools.

## Version 0.3.2
* Added additional null guards
* Fixed issue with Trickster Mobility 3 blocking allied spells in some cases.

## Version 0.3.1
* New localization extensions for BlueprintAbiltiies.
* Fixed issue with spell kenning unit part not handling multiple lists being added by the same feature correctly.

## Version 0.3.0
* New Components
	* SavingThrowBonusAgainstFavoredEnemy
		* Adds a saving throw bonus to all saves based on owners favored enemy rank.
* Deprecated Methods
	* Helpers.CreateBuff has been deprecated. Use Helpers.CreateBlueprint<BlueprintBuff>

## Version 0.2.1
* New Prerequisites
	* PrerequisiteInPlayerParty
		* Checks if unit is the player or a companion.

## Version 0.2.0
* Bugfixes
	* MagicalVestmentComponent
		* Should now calculate values correctly using the correct enhancement cap.

* New Actions
	* ContextActionCleaveAttack 
		* Supports adjacency checking from the current target with a mythic override.
	* AbilityCustomCleaveTTT 
		* Supports adjacency checking from the current target with a mythic override.

* New Components
	* ChangeUnitBaseSize
		* Support for base size changes that stack with existing size changes.
	* AddMythicSpellbook
		* Supports adding standalone mythic spellbooks with a CL defined by a context value.
	* IncreaseActivatableAbilitySpeed
		* Supports altering command speed of activatable abiltiies.
	* ContextConditionCasterHasResource
		* Supports checking a resource on the caster.
	* AddIncomingDamageBonus
		* Supports adding a damage bonus to incoming attacks with a specific modifier.
	* AddFlatCriticalRangeIncrease
		* Supports critical range increases that are not multiplied by improved critical.
	* AddIncomingDamageBonus
		* Supports incoming typed damage bonuses applied as a debuff.
	* ContextConditionCasterHasResource 
		* Condition that checks that a unit has the specified resource in the specified amount.
	* IncreaseCriticalRange 
		* Supports critical range increases which are not tied to a specific weapon.
	* EnduringSpellsTTT 
		* Supports configuring thresholds for both Enduring and Greater Enduring.
	* TricksterLoreNatureRestLootTriggerTTT
		* Rest trigger for Trickster World 3 mechanics in the Trickster Rework.
	* TricksterParryTTT 
		* Reimplementation of trickster parry to catch all typs of attacks.	

* New Mechanics Features
	* BypassSneakAttackImmunity
		* Fully implemented for use. Allows the bypassing of sneak attack and precision damage immunity.
	
* New Events
	* IActivatableAbilityGetCommandTypeHandler
		* Allows the altering of activatable ability command types at run time arbitrarily.
	* ICriticalRangeCalculatedHandler 
		* Allows the altering of critical ranges without interacting with improved critical.
## Version 0.1.1
* Fixed issue with the black blade part having in inaccessible static reference. This now exists as a hard coded ID to maintain save compatibility with older saves.
## Version 0.1.0
* Non final experimental release without stability guarantees