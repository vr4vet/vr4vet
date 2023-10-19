# NPC 👈(⌒▽⌒)👉

## Dialogue tree

- To use the dialogue tree on an NPC the DialogueBoxController and NPCManager scripts are added to the NPC object as components.

- A DialogueTree is a scriptable object that can be specified in Unity with dialogue, questions and answer boxed unique to that NPc.
  - This dialogue tree is then assigned to the NPC in the Dialogue Tree attribute defined in the NPC manager script.
  - The structs file is used by the DialogueTree.

- In the DialogueBoxController script, the Dialogue Text, NPC Name Text, Dialogue Box, Answer Box and Answer Objects are assigned.
  - The boxes are containers for the text objects, and the Dialogue text value is sat depending on the dialogue written in the Dialogue Tree object.
  - The Answer Objects is defined by the Answers written in the Dialogue Tree.

## Follow the Player

- Enable walking
  - Find the navigation window by clicking: window -> AI -> Navigation. In this window can you specify which areas of the map the NPC can walk on. So each time you make a change to the landscape of the map, you need to press bake here in the tab.
  - Ensure that the ground is walkable. Click on the ground, in the navigation tab go to Object. Make sure "Navigation Static" is enabled, Navigation area is set to Walkable.


- Who should the NPC follow?
  - First ensure that the prefab called GameManager is in the scene
  - Look at the script component called PlayerManager. Drag and drop the tracking target into the field called Player.There can only be one player in each scene (singleton). E.g. for the prefab called "XR Rig Advanced VR4VET" drag the CameraRig into the player-field of the script.


Inspiration/tutorials:

- [Creating your first animated AI Character! [AI #01] by TheKiwiCoder](https://www.youtube.com/watch?v=TpQbqRNCgM0)
- [ENEMY AI - Making an RPG in Unity (E10) by Brackeys](https://www.youtube.com/watch?v=xppompv1DBg&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=11)
  - Youtube video explaining the NavMeshAgent and how to get an enemy to follow the player
