# VR4VET Core
This repository holds a Unity project with demo Scenes and the core Prefabs to create VR experiences for the VR4VET project. The goal is to standardize all the experiences(games) and make the development of new ones easier and faster.

(Note: for a detail explanation on how to use each of the components check our Wiki)

## Instalation/Requirements

1. First you need to download Unity **2021.3.5f1** (this was the latest LTS at the time the project was started ).

2.Get a copy of VRI framework https://wiki.beardedninjagames.com/  (BNG Folder).

3. Clone the Develop branch using git clone. 

4. Populate "Assets/BNG Framework" with the conetent you downloaded.

5. After populating the BNG folder you may have to pull again.

## Using the Demo Scene

Under the VR4VET/Scenes you should see a scene called ‘MainTestScene’. This the existing Prefabs (more about them in the later section). 

#### Controls

* Primary button right hand: Main Menu
* Trigger right hand: UI Press
* Thumbstick left hand: teleportation
* Grip Button both hands: grab
 

## New Experience/Game instructions

If you are creating a new game you should create a new repository under this oganization. 

All Experiences/Games should use: Player, Tablet and Menu prefabs. All included in the Demo Scene under the 'Scenes' folder. You can make modifications to the prefabs if you need something specific to your game. If you think a feature is missing or if there is a bug in an existing Prefab, you should make an issue on this repository so someone can make that change.   


## Prefabs
Currently the detail explaination of how to use all the prefabs exist on the Wiki
