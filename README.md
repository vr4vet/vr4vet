# VR4VET Core
This repository holds a Unity project with demo Scenes and the core Prefabs to create VR experiences for the VR4VET project. The goal is to standardize all the experiences(games) and make the development of new ones easier and faster.

(Note: Both Game and Experience are used in this documentation to describe a VR experience where the user will try a profession)

## Instalation/Requirements

1. First you need to download Unity **2021.3.5f1** (this was the latest LTS at the time the project was started )

2. Clone the Develop branch using git clone 

3. Go to ‘Window’ -> ‘Package manage’ and make sure **Open XR Plugin** is installed. If  already there ignore this step


## Using the Demo Scene

Under the VR4VET/Scenes you should see a scene called ‘Template 1’. This the existing Prefabs (more about them in the later section). 

This Scenes demos 3 simple tasks that you can check using your tablet. If you click on the task it will also show you the way towards that task with green arrows(unless you are already too close). 

#### Controls

* Primary button right hand: Tablet
* Secondary button right hand: Main Menu
* Trigger right hand: UI Press
* Thumbstick left hand: teleportation
* Grip Button both hands: grab
 

This Scene is going to get a lot of changes after transitioning to VR Interaction Framework



## New Experience/Game instructions

If you are creating a new game you should create a new repository under this oganization. 

All Experiences/Games should use: Player, Tablet and Menu prefabs. All included in the Demo Scene under the 'Scenes' folder. You can make modifications to the prefabs if you need something specific to your game. If you think a feature is missing or if there is a bug in an existing Prefab, you should make an issue on this repository so someone can make that change.   


## Prefabs
* Pause Menu
* 360 Video Player: https://github.com/vr4vet/vr4vet/blob/main/360VideoREADME.md
* Tablet: https://github.com/vr4vet/vr4vet/blob/main/TabletREADME.md
  - Navigation manager
  - Skill Manager
  - Task and Activities
* XR Player:
Currently the player has the tablet under the left hand (and you can choose if you want it on your hand or floating in front of you )
This prefab will be changed once we change platform (XRI toolkit -> VR Interaction Framework)

## Branching 
The main branch should not be updated directly. All feature branches should be merged into a develop branch and after each release, the release branch should be merged into both main and develop.

The develop branch is where changes to the basic functionalities should be added. Branches with new Prefabs or functionalities should be forked from this Branch.

New Games/Projects belong in a different repository

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

### Changing Language
To change language, when running the game in the unity editor, in the game view in the top right there should be a dropdown to select the current language.
