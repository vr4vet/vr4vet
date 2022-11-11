# vr4vet


## Translation System
The unity package 'Localization 1.4.2' is used for localization and translation (found under package manager > unity registry).\
Documentation can be found at: https://docs.unity3d.com/Packages/com.unity.localization@1.4/manual/index.html

The default language is set to English, and is configurable by modifying both:\
Project Settings > Localization > Specific Locale Selector > Locale Id\
and Project Settings > Localization > Project Locale Identifier

The translation tables can be found at:\
Window > Asset Management > Localization Tables\
Note unity will only load 1 unique localization table per asset type (e.g. string, texture etc) per scene.\
![localization_assetTable](https://user-images.githubusercontent.com/112614548/201331878-29aab447-3a25-4215-b933-567068ad65be.JPG)
![localization_stringTable](https://user-images.githubusercontent.com/112614548/201331898-93e537e8-fb66-4fc0-be55-4550041d510f.JPG)


### Localization Scene Controls
The Localization system allows you to work directly with scene components and will record any changes made directly to a component for the currently active Locale. To do this you will need to first set up the project. Open the Localization Scene Controls window (menu: Window > Asset Management > Localization Scene Controls). Set the Asset Table field to the table collection that was just created. Any asset changes made now are recorded into the selected Asset Table.\
![localizaton_scene_controls](https://user-images.githubusercontent.com/112614548/201331917-6a5fb404-280f-4f85-88b6-df8482bbcc91.JPG)

1- In the Asset Table field select your table collection.\
2- Any asset changes you make now are recorded into the selected Asset Table.\
3- The workflow is: Select the current locale in the Localization Scene Controls, modify assets to conform to the selected locale (e.g. if 'French' is selected as current locale, then edit the asset to be french, the edited fields that are tracked by the localization system will highlight in GREEN - otherwise it is not working).\
![localizaton_green_field](https://user-images.githubusercontent.com/112614548/201331942-479f7ed3-c646-440d-93ae-d4f4946319b2.JPG)
