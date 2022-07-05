# VR4VET Core
This repository holds the demo Scene and core Prefabs that should be cloned before starting a new Experience. if you are creating a new Experience you should clone this project and start a new repository. 

## New Project/Game instruccions

Please read the 'Branching' section if you are not sure in which repo/brach you should work. 

All Experiences should use: Player, Tablet and Menu prefabs. All included in the Demo Scene under the 'Scenes' folder. You can make modifications to the prefabs if you need something spesific to your game. If you want a new features or if there is a bug in an existing Prefab, you should make an issue so somone can make that change in the develop branch.       

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



