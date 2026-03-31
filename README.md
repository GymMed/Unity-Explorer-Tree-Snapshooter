<h1 align="center">
    UnityExplorer Tree Snapshooter
</h1>
<br/>
<div align="center">
  <img src="https://raw.githubusercontent.com/GymMed/Unity-Explorer-Tree-Snapshooter/refs/heads/main/preview/images/Logo.png" alt="Logo"/>
</div>

<div align="center">
	<a href="https://thunderstore.io/c/outward/p/GymMed/Unity_Explorer_Tree_Snapshooter/">
		<img src="https://img.shields.io/thunderstore/dt/GymMed/Unity_Explorer_Tree_Snapshooter" alt="Thunderstore Downloads">
	</a>
	<a href="https://github.com/GymMed/Unity-Explorer-Tree-Snapshooter/releases/latest">
		<img src="https://img.shields.io/thunderstore/v/GymMed/Unity_Explorer_Tree_Snapshooter" alt="Thunderstore Version">
	</a>
	<a href="https://github.com/sinai-dev/UnityExplorer/releases/latest">
		<img src="https://img.shields.io/badge/UnityExplorer-v4.x.x-D4BD00" alt="Mods Communicator Version">
	</a>
</div>
<div align="center">
    A mod that captures GameObject hierarchy and component state to text files. Useful for Debugging with AI.
</div>

<details>
<summary>
Preview
</summary>
  <img src="https://raw.githubusercontent.com/GymMed/Unity-Explorer-Tree-Snapshooter/refs/heads/main/preview/images/panel.png" alt="Logo"/>
</details>

## Dependencies

This mod requires:
- [UnityExplorer](https://github.com/sinai-dev/UnityExplorer) - Required for the Inspector panel
- [UniverseLib](https://github.com/sinai-dev/UniverseLib) - UI framework used by UnityExplorer

## How It Works

1. **Harmony Patch** - Patches `InspectorPanel.ConstructPanelContent` to add a "Snapshot" button in UnityExplorer's Inspector title bar
2. **UI Panel** - Opens `SnapShooterPanel` with:
   - Current selection name and instance ID
   - Save path input field (editable)
   - "Snapshot Selection" button
   - Log output area
3. **Snapshot Process** - When clicking "Snapshot Selection":
   - Gets the currently inspected GameObject from UnityExplorer's InspectorManager
   - Builds the parent chain (path from scene root to selected object)
   - Builds the children hierarchy (all descendants with instance IDs)
   - Reads all component state via reflection for each GameObject
   - Formats and saves to text file

### How to Use

1. Install UnityExplorer and this mod in `Game/BepInEx/plugins/`
2. Open UnityExplorer (default key: F7)
3. Select a GameObject in the Inspector panel
4. Click the "Snapshot" button in the Inspector title bar
5. Edit the save path if desired, or use the default location
6. Click "Snapshot Selection" to save

## Snapshot Details

The snapshot output includes:

### 1. Parent Hierarchy
```
=== PARENT HIERARCHY ===
Root (InstanceID: 100)
  Level1 (InstanceID: 200)
    Player (InstanceID: 300)
```

### 2. Children Hierarchy
```
=== CHILDREN HIERARCHY ===
[0] Player (InstanceID: 300)
  [1] Child1 (InstanceID: 301)
  [1] Child2 (InstanceID: 302)
```

### 3. Components
```
=== COMPONENTS ===
  Transform (InstanceID: 456)
  Rigidbody (InstanceID: 789)
```

### 4. Reflection State
```
=== GAME OBJECT REFLECTION (ALL CHILDREN) ---

--- Player (InstanceID: 300) [SELECTED] ---
  Transform (InstanceID: 456):
    property m_LocalPosition: (0, 0, 0)
    property m_LocalRotation: (0, 0, 0, 1)
  Rigidbody (InstanceID: 789):
    property mass: 1

--- Child1 (InstanceID: 301) ---
  Transform (InstanceID: 1001):
    property m_LocalPosition: (1, 2, 3)
```

### 5. Outside References
When a field references a GameObject outside the selection, it's marked:
```
field m_Parent: [OUTSIDE_REFERENCE: InstanceID=999, Type=Transform] ParentName
```

## Default Snapshot Location

By default, snapshots are saved to:
```
{Mod Location}/Snapshots/TreeSnapShooter_{GameObjectName}_{yyyy-MM-dd_HH-mm-ss}.txt
```

Example:
```
Game/BepInEx/plugins/UnityExplorerTreeSnapShooter/Snapshots/TreeSnapShooter_Player_2026-03-27_10-30-00.txt
```

Users can override the save path by entering a custom directory or file path in the input field.

## How to set up

To manually set up, do the following

1. Create the directory: `Game\BepInEx\plugins\UnityExplorerTreeSnapShooter\`.
2. Extract the archive into any directory(recommend empty).
3. Move the contents of the plugins\ directory from the archive into the `BepInEx\plugins\UnityExplorerTreeSnapShooter\` directory you created.
4. It should look like `Game\BepInEx\plugins\UnityExplorerTreeSnapShooter\UnityExplorerTreeSnapShooter.dll`
   Launch the game.

### If you liked the mod leave a star on [GitHub](https://github.com/GymMed/Unity-Explorer-Tree-Snapshooter) it's free
