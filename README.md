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

## Using Prefabs

Some of the prefabs have existing documentation

* 360 Video Player: https://gitlab.stud.idi.ntnu.no/imtel/developers/360videoplayer
* Tablet: https://gitlab.stud.idi.ntnu.no/imtel/developers/tablet


