# VR4VET Core
This repository holds a Unity project with demo Scenes and the core Prefabs to create VR experiences for the VR4VET project. The goal is to standardize all the experiences(games) and make the development of new ones easier and faster.

(Note: Both Game and Experience are used in this documentation to describe a VR experience where the user will try a profession)

## New Experience/Game instructions

If you are creating a new game you should create a new repository under this oganization. 

All Experiences/Games should use: Player, Tablet and Menu prefabs. All included in the Demo Scene under the 'Scenes' folder. You can make modifications to the prefabs if you need something specific to your game. If you think a feature is missing or if there is a bug in an existing Prefab, you should make an issue on this repository so someone can make that change.     

## Prefabs 
* 360 Video Player
* Pause Menu
* Tablet
  - Navigation manager
  - Skill Manager
  - Task and Activities

## Branching 
The main branch should not be updated directly. All feature branches should be merged into a develop branch and after each release, the release branch should be merged into both main and develop.

The develop branch is where changes to the basic functionalities should be added. Branches with new Prefabs or functionalities should be forked from this Branch.

New Games/Projects belong in a different repository





