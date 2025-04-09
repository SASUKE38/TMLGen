# TMLGen

 This tool generates the TML file for a given Baldur's Gate 3 cutscene, allowing the cinematic to be easily edited in an unlocked version of the toolkit (such as [MoonGlasses](https://www.nexusmods.com/baldursgate3/mods/12308?tab=description)).

## Usage

The tool requires the path to your unpacked game directory (as unpacked by the [Baldur's Gate 3 Modder's Multitool](https://github.com/ShinyHobo/BG3-Modders-Multitool)) along with the source file of the timeline you wish to edit.
Additionally, you may select the timeline data to use manually. The definitions below detail the data the tool uses.

- Unpacked Data Directory: Top level of unpacked game data as unpacked by the Baldur's Gate 3 Modder's Multitool. Likely called UnpackedData.
- Source File: .lsf source file of the timeline you wish to edit. Likely located in a Public\\<mod name\>\Timeline\Generated directory.

The following directory is only used when setting the tool to override timelines.

- Game Data Directory: Contains the .pak files the game uses. The path likely ends with Baldurs Gate 3\Data.

The following files are used only for manual selection and can be ignored for automatic generation.

- Generated Dialog Timelines File: .lsf file that contains information linking the timeline and dialog. Likely located in a Public\\<mod name\>\Content\Generated\\[PAK]_GeneratedDialogTimelines directory.
- Dialogs Binary File: .lsf file that contains the compiled dialog data. Likely located in a subdirectory of the Mods\\<mod name\>\Story\DialogsBinary directory.
- Dialogs File: .lsj file that contains the raw dialog data. Likely located in a subdirectory of the Mods\\<mod name\>\Story\Dialogs directory.
- Timeline Templates Directory: Not required (will not be present for timelines with no templates). Contains the templates a timeline uses. Likely located in a Public\\<mod name\>\TimelineTemplates folder and will be named with the timeline's GUID.

### Overriding a Timeline

#### Automatic Overriding

The tool supports overriding timelines if a mod name and game data path are provided. Note that the mod name must appear as it does in file paths. If the editor is open when you use the tool, you might need to restart it for the override to be recognized.

#### Manual Overriding

If you want to override a timeline manually, you can do so with the steps below.

1. Locate the timeline files in the Timeline Data\\<timeline name\> folder created by the tool.
2. Place the source and scene files in your mod's Public\\<mod name\>\Timeline\Generated folder.
3. Place the templates folder (whose name will be the timeline's GUID - if no such folder exists, skip this step) in your mod's Public\\<mod name\>\TimelineTemplates folder.
4. Place the .tml and _ref.json files in your mod's Editor\Mods\\<mod name\>\Timeline\Generated folder.
5. Place the Generated Dialog Timelines file (.lsf file whose name ends with _GDT) in your mod's Public\\<mod name\>\Content\Generated\\[PAK]_GeneratedDialogTimelines folder.
6. Restart the editor if it is open.

## Known Limitations and Workarounds

- The tool does not support Combat Timelines.
- The tool currently only supports English for creating timeline reference (_ref.json) files.
- Generation does not give actors descriptive names, relying instead on names like "Additional 2" or "Initiator 1." You can find their actual names by referencing the dialog's speaker list or by hovering over the actor's track.
- The timeline's initial location is inferred. If it is incorrect, you can set it by hand in the editor.
- Slot materials might not work correctly as is. To overcome this, try the following steps:
  1. Select the actor that owns the slot material.
  2. Locate the Visual Resource ID property in the sidebar and copy the associated GUID.
  3. Open the generated .tml file in a text editor.
  4. Use the Find feature (CTRL + F) to search for "CharacterVisualResourceId" (no quotes).
  5. Identify which instance of this string is associated with the slot material. It should be under a track that has the name of the actor in question. If there are multiple slot materials, they will be ordered in the same way they appear in the toolkit.
  6. Replace the CharacterVisualResourceId GUID of the slot material with the one you copied from the editor. Close and reopen the timeline (do not refresh; this will overwrite the change you made).
  7. If the slot material still does not work, try adding another version of the slot material.
  8. Open the .tml in a text editor once again.
  9. Locate the slot material you added as you did above.
  10. Copy this slot material's CharacterVisualResourceId and replace the slot material of the one you want to get working.
  11. Note that some actors support different versions of the same slot material; it might be unclear which to use for this process, so multiple tries with this method might be necessary.
- Actors taken from the world might not work correctly as is. To overcome this, try the following steps:
  1. Make a backup of the .tml file.
  2. Note the name of the actor when you hover over it.
  3. Delete the actor and re-add it.
  4. Select the actor in the sidebar.
  5. If the Parent Template ID is not null (all zeros), copy it.
  6. Undo the changes or restore the backup of the .tml file.
  7. Open the .tml file in a text editor.
  8. Use the Find feature (CTRL + F) to search for the name of the actor.
  9. Replace the ParentTemplateId with the one you copied. If no such attribute is present, add it with the form ParentTemplateId="00000000-0000-0000-0000-000000000000"

## Credits

- Thank you to [magnetuning](https://next.nexusmods.com/profile/magnetuning?gameId=3474) for research into the BG3 dialog system. This would not be possible without his [dialog guide](https://wiki.bg3.community/en/Tutorials/new-voice-lines) and [sample mod](https://www.nexusmods.com/baldursgate3/mods/10086).
- Thank you to Plouton, [Em](https://hellions-heart.tumblr.com), and Curious_Cactus for testing.
- This tool uses [LSLib](https://github.com/Norbyte/lslib), which is created by Norbyte and licensed under the MIT License.
