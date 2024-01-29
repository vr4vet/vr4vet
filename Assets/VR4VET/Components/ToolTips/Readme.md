# Tooltips
A prefab used to display tooltips with various contents.

# Importing the prefab
1. Download the prefab and export as a unitypackage.
2. Open the project you want to import to.
3. Right click in the project panel and select "Import Packages" -> "Custom Package"
4. Select the TooltipPrefab.unitypackage file.
5. You should now have a folder named "Tooltips", with a few subfolders.
6. In the "Prefabs"-folder, drag the SimpleToolTip prefab onto the object in the hierarchy you want to have a tooltip.
7. Select the "CenterEyeAnchor" in the scene tab as the Canvas Event Camera.
8. Drag the XR Rig from your scene to the Player variable of the Tooltip Script component.
9. Drag the TooltipManager Prefab into your scene.

# Using the tooltips from the editor (static tooltips)
1. Edit the header and text content fields.
2. Optionally adjust the display conditions of the tooltip.

# Using the tooltips through scripts (dynamic tooltips)
1. Get a reference to the tooltip. This is usually done through `TooltipScript tooltip = ObjectWithTooltip.Find("SimpleToolTip");`
2. To activate the tooltip, use `tooltip.Activate()`. Note that unless it is disabled in the tooltip manager, this will also close all other tooltips.
3. To disable a tooltip, use `tooltip.Deactivate()`.
4. To alter the contents of a tooltip, assign a string value to the header or text content. Examples: `tooltip.TextContent = "New Tooltip Content"` `tooltip.Header = "New Tooltip Header"`.
5. Any alterations to content, position, or activation status will update dynamically while running.