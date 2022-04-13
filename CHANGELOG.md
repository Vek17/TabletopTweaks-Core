## Versions 0.2.0
* New Components
	* ChangeUnitBaseSize
		* Support for base size changes that stack with existing size changes.
	* AddMythicSpellbook
		* Supports adding standalone mythic spellbooks with a CL defined by a context value.
	* IncreaseActivatableAbilitySpeed
		* Supports altering command speed of activatable abiltiies.
* New Events
	* IActivatableAbilityGetCommandTypeHandler
		* Allows the altering of activatable ability command types at run time arbitrarily.
## Version 0.1.1
* Fixed issue with the black blade part having in inaccessible static reference. This now exists as a hard coded ID to maintain save compatibility with older saves.
## Version 0.1.0
* Non final experimental release without stability guarantees