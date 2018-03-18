System for creation of squads (such as for use with a squad of enemies) for a Unity3D third-person 3D shooter. Custom editors for objects included to allow for UI-based set-up within Unity, including GUI for visualizing spawn positions, unit locations, etc.


SquadManager: Single manager to handle all of game's SquadTriggers
SquadTriggers: Triggers (hitboxes) that define/restrict Squad placement, and tells Squads when to spawn
Squad: Contains parameters to define the squad, and spawn the objects it contains

SquadFormation: Formation presets for automatic generation of spawning patterns - provides example for linking units (linked list)

UnitCursor: Helper