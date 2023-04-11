## Version 0.6.4
* New WeaponSizeChangeTTT component.

## Version 0.6.3
* New IDispelMagicHandler added to handle entire dispel checkes instead of just dispelMagicRules.
* New PolymorphDamagePropertyTransfer that handled alignment properties correctly.
* DestructiveDispelComponent has been updated with new event.

## Version 0.6.2
* Mod compatability fix.

## Version 0.6.1
* Fixed issue with PrerequisiteSpellKnown not working correctly in all cases.
* Fixed issue with Trickster domains not 
* Support for age effects
* Support for spell curse
* Support for Split Hex toggle
* Breaking change to bloodline tools to support bloodrager correctly


## Version 0.5.14
* Added ContextConditionItemSource to check if an ability is cast from an item.
* Added WeaponGroupAttackBonusTTT to provide working versions of owlcat components.
* Added WeaponGroupDamageBonusTTT to provide working versions of owlcat components.
* Added SplitHex components.
* Added BewitchingReflex components.

## Version 0.5.13
* AddAbilityUseTargetTriggerTTT adds support for triggers on immune spells.
* Fixed context stripping from BurningMagic.

## Version 0.5.12
* More SaveGameFix options.

## Version 0.5.11
* Adjusted feat tools to better handle metamagic.
* Support for Oracle Burning Magic.
* Support for Winter Witch features.

## Version 0.5.10a
* Added fallthrough case for PrerequisiteDeityAlignment in case someone breaks deity alignment checks.

## Version 0.5.10
* Added support for ChainChallenge.
* Added AddAdditionalLimbConditional which supports extra natural attacks with checked facts.
* Added Wild shape tools to make adding wild shaped features easier.
* Added support for glossery tag creation.
* Added support for metamagic glossery tags.
* Fixed a bug where hidden activatable abiltiies were incorrectly handled.
* FeatTools.ConfigureAsTeamworkFeat added to setup all tactician and shared teamwork mechanics.

## Version 0.5.9
* Updated for 2.1.0
* Moved AddOutgoingWeaponDamageBonus to post crit damage system
* Added support for more ActivatableAbilityGroups with AdditionalActivatableAbilityGroups

## Version 0.5.8
* Support for twin spell metamagic
* Fixed issue with aberrant bloodline and permanant polymorph durations

## Version 0.5.7
* Support for descriptor immunities

## Version 0.5.6
* Fix for MechanicActionBarSlotActivableAbility

## Version 0.5.5
* Fix for 2.0.4j

## Version 0.5.4
* Contains fix for broken IAfterRulebookEventTriggerHandler, and IBeforeRulebookEventTriggerHandler events

## Version 0.5.3
* Removed old GetComponent method as it has been replaced by owlcat tooling.
* ContextArcaneSpellFailureIncrease now handles shields more correctly.

## Version 0.5.2
* New Components
	* AddAdditionalWeaponDamageOnHit
		* Triggers additional damage on conditions similiar to AddInitiatorAttackWithWeaponTrigger but packaged into a weapon damage bundle.

## Version 0.5.1
* BloodlineTools now properly handles Nine Tailed Hier
* CompositeProperties now support multiplication
* New Components
	* ContextActionApplyBuffRanks
		* Adds buffs like ContextActionApplyBuff but with the ability to apply multiple ranks of a buff.
* New Utilities
	* VenderTools
		* Adds methods for adding scrolls and potions to vender lists dynamicly.

## Version 0.5.0
* Release for 2.0.0
* Increased compatability with other mods that add ModifierDescriptors
* Updated logic for AddOutgoingWeaponDamageBonus
* Fixed issue with ApendToArray's null handling

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