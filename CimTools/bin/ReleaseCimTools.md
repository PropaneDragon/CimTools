
# CimTools


## File.Path

Handles system paths


### M:CimTools.GetModPath(workshopId, modName)

Gets the full path of the mod

| Name | Description |
| ---- | ----------- |
| workshopId | *System.UInt64*<br>The workshop ID of the mod |
| modName | *System.String*<br>The name of the mod |


#### Returns

The full path of the mod


## File.PersistentOptions

Persistent options are not saved inside the save file for a level, and persist regardless of the level that is loaded. These are good for saving options for a mod, which do not need to interact with elements within a certain level, or elements that can change between saved files. All data stored in here is saved to the same folder as the game executable. This can be edited at any time by the user, and is not protected. It is stored as XML.


### M:CimTools.GetValue``1(name, value, groupName)

Gets a stored value of type T.

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The unique name of the data. |
| value | *``0@*<br>The output value of the data. |
| groupName | *System.String*<br>The name of the group to load the data from. |


#### Returns

Whether the data could be retrieved or not.


### M:CimTools.GetValues``1(groupName)

Gets all values of type T under the specified group name.

| Name | Description |
| ---- | ----------- |
| groupName | *System.String*<br>The group to search under. |


#### Returns




### M:CimTools.Instance

Gets an instance of the SavedOptions


#### Returns

An instance of SavedOptions


### M:CimTools.Load

Load all options from the disk.


#### Returns

Whether loading was successful


### F:CimTools.OptionVersion

The current version of the PersistentOptions class. This is for doing upgrades from older versions of the class if needed.


## File.PersistentOptions.PersistentOptionError

Any errors that could occur in the options.


### F:CimTools.CastFailed

Could not cast the option to the specified type.


### F:CimTools.FileNotFound

The saved data file could not be found.


### F:CimTools.GroupNotFound

The group could not be retrieved.


### F:CimTools.LoadFailed

Could not load the data.


### F:CimTools.NoError

No error has occurred.


### F:CimTools.OptionNotFound

The option could not be retrieved.


### F:CimTools.SaveFailed

Could not save the data.


### M:CimTools.File.PersistentOptions.Save

Saves all options to the disk. Make sure you've updated the options first.


### F:CimTools.File.PersistentOptions.SavedData

Data that gets saved to disk. You shouldn't have to modify this manually, and instead should use the SetValue and GetValue methods.


### M:CimTools.File.PersistentOptions.SetInstance(optionManager)

Change the instance used for the options.

| Name | Description |
| ---- | ----------- |
| optionManager | *CimTools.File.PersistentOptions*<br>The SavedOptions to replace the existing instance |

### M:CimTools.File.PersistentOptions.SetValue``1(name, value, groupName)

Sets a value to be saved to disk automatically.

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The unique name of the data. |
| value | *``0*<br>The input value of the data. |
| groupName | *System.String*<br>The name of the group to save this data in. |

## File.SavedElement

Container for saving data to disk.


### F:CimTools.name

Unique item name


### F:CimTools.value

Value of the item


## File.SavedGroup

Container for saving groups of data to disk.


### F:CimTools.elements

SavedElements in the group


### F:CimTools.name

Unique group name


## File.Version

Handles everything to do with the mod version number


### M:CimTools.Build

Build version (1.2.[35].623)


#### Returns

The build number


### M:CimTools.Delimited(delimiter, upTo)

Returns a delimited version string

| Name | Description |
| ---- | ----------- |
| delimiter | *CimTools.File.Version.Limit*<br>The delimiter to place between version numbers. |
| upTo | *System.String*<br>Return a version number up to this limit. |


#### Returns

A delimited version string


## File.Version.Limit

How far to limit the version numbering to.


### F:CimTools.Build

Build version (1.2.[35].623)


### F:CimTools.Major

Major version ([1].2.35.623)


### F:CimTools.Minor

Minor version (1.[2].35.623)


### F:CimTools.Revision

Revision version (1.2.35.[623])


### M:CimTools.File.Version.Major

Major version ([1].2.35.623)


#### Returns

The major number


### M:CimTools.File.Version.Minor

Minor version (1.[2].35.623)


#### Returns

The minor number


### M:CimTools.File.Version.Revision

Revision version (1.2.35.[623])


#### Returns

The revision number


## Panels.UpdatePanel

A speech bubble which contains the latest changes to your mod. The bubble can be clicked and expanded to reveal new changes, rather than taking up loads of space.


### F:CimTools.m_changelogDownloader

Set and get a Changelog downloader. By default this uses the default Changelog instance, but you can pass your own through here if you wish to use your own. Please note that if you are using the default Changelog instance you will need to set it up for your mod, otherwise you'll get no changes.


### F:CimTools.m_updatedContentMessage

The initial message content when an update has been detected.


### F:CimTools.m_updatedTitleMessage

The initial message title when an update has been detected.


### M:CimTools.setPositionSpeakyPoint(position)

Sets the position of the panel based on the "speaky point" of the speech bubble it creates

| Name | Description |
| ---- | ----------- |
| position | *UnityEngine.Vector2*<br>Position to set the panel to |

## Settings

Global settings for CimTools. Set these up before doing anything with the tools, or elements may not work properly.


### F:CimTools.ModAssembly

The assembly to get the version name from. Use Assembly.GetExecutingAssembly() if unsure. Required for: Version.


### F:CimTools.ModName

The name of the mod you wish to use. Required for: Path, PersistentOptions


### F:CimTools.WorkshopID

The workshop ID of the mod. Required for: Changelog.


## Utilities.ModOptionUtilities

Handles options on the mod option panel ingame


### M:CimTools.CreateOptions(helper, groupName)

Creates options on a panel using the helper

| Name | Description |
| ---- | ----------- |
| helper | *ICities.UIHelperBase*<br>The UIHelper to put the options on |
| groupName | *System.String*<br>The title of the group in the options panel. |

### M:CimTools.LoadOptions

Manually load the UI options onto the existing panel elements.


### E:CimTools.OnOptionPanelSaved

When the options have been saved by the user.


### M:CimTools.SaveOptions

Manually save the UI options to the persistent options. When this is called by the class internally, you can get the event using OnOptionPanelSaved()


## Utilities.OptionPanelSaved

When the user has saved the options


## Utilities.OptionsCheckbox

Checkbox option


### M:CimTools.Create(helper)

Create the element on the helper

| Name | Description |
| ---- | ----------- |
| helper | *ICities.UIHelperBase*<br>The UIHelper to attach the element to |

### .value

The default value of the object, or the saved value if loaded.


## Utilities.OptionsDropdown

Dropdown option


### M:CimTools.Create(helper)

Create the element on the helper

| Name | Description |
| ---- | ----------- |
| helper | *ICities.UIHelperBase*<br>The UIHelper to attach the element to |

### F:CimTools.options

All available options to select in the dropdown


### .value

The default value of the object, or the saved value if loaded.


## Utilities.OptionsItemBase

Base class for all option panel options.


### M:CimTools.Create(helper)

Create the element on the helper

| Name | Description |
| ---- | ----------- |
| helper | *ICities.UIHelperBase*<br>The UIHelper to attach the element to |

### F:CimTools.enabled

Whether the option is enabled or not


### F:CimTools.readableName

The name that appears on the UI.


### F:CimTools.uniqueName

The unique option name. Can't clash with any other option names or you'll lose data.


## Utilities.OptionsSlider

Slider option


### M:CimTools.Create(helper)

Create the element on the helper

| Name | Description |
| ---- | ----------- |
| helper | *ICities.UIHelperBase*<br>The UIHelper to attach the element to |

### F:CimTools.max

Upper bound for the slider


### F:CimTools.min

Lower bound for the slider


### F:CimTools.step

The amount to step when the user slides the slider


### .value

The default value of the object, or the saved value if loaded.


## Utilities.SpriteUtilities

Utilities for sprite handling. Takes care of atlases and sprites. Atlases are a large map of many small sprites. Cities handles all individual sprites using a single large atlas, and each sprite has a location on the atlas.


### M:CimTools.AddSpriteToAtlas(dimensions, spriteName, atlasName)

Creates a new sprite using the size of the image inside the atlas.

| Name | Description |
| ---- | ----------- |
| dimensions | *UnityEngine.Rect*<br>The location and size of the sprite within the atlas (in pixels). |
| spriteName | *System.String*<br>The name of the sprite to create |
| atlasName | *System.String*<br>The name of the atlas to add the sprite to. |


#### Returns




### M:CimTools.FixTransparency(texture)

Copy the values of adjacent pixels to transparent pixels color info, to remove the white border artifact when importing transparent .PNGs.

| Name | Description |
| ---- | ----------- |
| texture | *UnityEngine.Texture2D*<br> |

### M:CimTools.GetAtlas(atlasName)

Returns a stored atlas.

| Name | Description |
| ---- | ----------- |
| atlasName | *System.String*<br>The name of the atlas to return. |


#### Returns




### M:CimTools.InitialiseAtlas(texturePath, atlasName)

Creates a new atlas from a texture and a name.

| Name | Description |
| ---- | ----------- |
| texturePath | *System.String*<br>The full path to the texture. |
| atlasName | *System.String*<br>The name to give the atlas. Used for finding and using later. |


#### Returns

Whether the atlas was created successfully.


## Utilities.UIUtilities

Utilities for adding items to the UI


### M:CimTools.CreateButton(parent)

Creates a button on the component

| Name | Description |
| ---- | ----------- |
| parent | *ColossalFramework.UI.UIComponent*<br>The component to add the button to |


#### Returns

A new button


### M:CimTools.CreateCheckBox(parent)

Creates a checkbox on the component

| Name | Description |
| ---- | ----------- |
| parent | *ColossalFramework.UI.UIComponent*<br>The component to add the checbox to |


#### Returns

A new checkbox


### M:CimTools.CreateColorField(parent)

Creates a color field on the component

| Name | Description |
| ---- | ----------- |
| parent | *ColossalFramework.UI.UIComponent*<br>The component to add the color field to |


#### Returns

A new color field


### M:CimTools.CreateDropDown(parent)

Creates a dropdown on the component

| Name | Description |
| ---- | ----------- |
| parent | *ColossalFramework.UI.UIComponent*<br>The component to add the dropdown to |


#### Returns

A new dropdown


### M:CimTools.CreateTextField(parent)

Creates a text field on the component

| Name | Description |
| ---- | ----------- |
| parent | *ColossalFramework.UI.UIComponent*<br>The component to add the text field to |


#### Returns

A new text field


### .defaultAtlas

The default game atlas


### M:CimTools.GetAtlas(name)

Gets an atlas by name

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The atlas name to retrieve |


#### Returns

The named atlas, or null if none is found.


### M:CimTools.ResizeIcon(icon, maxSize)

Resizes a sprite to fit within certain bounds. It doesn't stretch the sprite and keeps the correct ratio.

| Name | Description |
| ---- | ----------- |
| icon | *ColossalFramework.UI.UISprite*<br>The sprite to resize |
| maxSize | *UnityEngine.Vector2*<br>The maximum size of the sprite |

## Workshop.Changelog

Used for grabbing change logs from the Steam Workshop. This will automatically get the most recent change log on the page and return it in either a list or a string. The workshop changelogs must be formatted in a specific way in order for them to be correctly formatted when getting the output from the class. Every change must be within [list][/list] tags with [*] before each item when entering them into the workshop.


### .ChangesList

A list of change logs.


### .ChangesString

A formatted string of change logs. Each change log is separated by \n\n.


### M:CimTools.DownloadChangelog

Downloads the latest changelog from a workshop item using the ID provided. Please note that this will stop the calling thread while it downloads, but you can guarantee the download will be complete when the thread resumes.


### M:CimTools.DownloadChangelogAsync

Downloads the latest changelog from a workshop item using the ID provided. Please note that while this works on a separate thread, you can't guarantee the download will be complete immediately. You should check for a complete download using m_downloadCompleted before trying to get data from it.


### .DownloadComplete

If downloading Async, this will return whether the download has completed from that request. If the download fails it will still be false.


### .DownloadError

Whether an error has occured while downloading the most recent changes.


### .DownloadInProgress

If downloading Async, this determines whether or not there is an ongoing download.


### M:CimTools.ExtractData(rawData)

Extracts data from a HTML page. This will only work on the Steam Workshop due to certain div IDs. You shouldn't need to call this yourself really, and should only be used if you really really need to use it.

| Name | Description |
| ---- | ----------- |
| rawData | *System.String*<br> |

### M:CimTools.Instance

Gets and creates an instance of the Changelog class, if required.


#### Returns

A Changelog class


### F:CimTools.m_changeList

A list of changes which is populated upon download.


### F:CimTools.m_colouriseTags

Whether or not to replace HTML tags with colours.


### F:CimTools.m_downloadComplete

Whether the download is complete or not.


### F:CimTools.m_downloadError

Whether the download has encountered an error.


### F:CimTools.m_downloadInProgress

Whether a download is in progress or not.


### F:CimTools.m_instance

The class instance


### F:CimTools.m_rawChanges

A string which contains all changes. Populated upon download.


### F:CimTools.m_tagsToColourise

A list of tags to replace with colours.


### F:CimTools.m_webClient

The web client that accesses the community and downloads the changes

