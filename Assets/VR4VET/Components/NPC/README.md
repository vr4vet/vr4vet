# NPC 👈(⌒▽⌒)👉

## Dialogue tree

- To use the dialogue tree on an NPC the DialogueBoxController and NPCManager scripts are added to the NPC object as components.

- A DialogueTree is a scriptable object that can be specified in Unity with dialogue, questions and answer boxed unique to that NPc.
  - This dialogue tree is then assigned to the NPC in the Dialogue Tree attribute defined in the NPC manager script.
  - The structs file is used by the DialogueTree.

- In the DialogueBoxController script, the Dialogue Text, NPC Name Text, Dialogue Box, Answer Box and Answer Objects are assigned.
  - The boxes are containers for the text objects, and the Dialogue text value is sat depending on the dialogue written in the Dialogue Tree object.
  - The Answer Objects is defined by the Answers written in the Dialogue Tree.
 
## Utilize NPC dialogue

- To use the NPC dialogue there needs to be defined a *Colliding Object* for the NPC in the NPCManager script. (Currently we have been using the *HolsterRight* object from the XR Rig Advanced, which has worked well, but other objects can probably be used instead). You can find the NPC script on the gameObject called CollisionTriggerHandler. 
  
- At the time a default "dummy" dialogue tree with all the needed objects is included in the NPC prefab, but at a later stage this needs to be changed.

### Modifactions

- You can change (and should) the dialog tree
- You can also adjust the radius for when the dialogue is triggered. We advise you to keep this radius less than the personal space factor for NPC following mentiond below.

## Follow the Player

- Enable walking
  - Find the navigation window by clicking: window -> AI -> Navigation. In this window can you specify which areas of the map the NPC can walk on. So each time you make a change to the landscape of the map, you need to press bake here in the tab.
  - Ensure that the ground is walkable. Click on the ground, in the navigation tab go to Object. Make sure "Navigation Static" is enabled, Navigation area is set to Walkable.

- Who should the NPC follow?
  - First ensure that the prefab called GameManager is in the scene
  - Look at the script component called PlayerManager. Drag and drop the tracking target into the field called Player.There can only be one player in each scene (singleton). E.g. for the prefab called "XR Rig Advanced VR4VET" drag the CameraRig into the player-field of the script.

### Modfications

- You can change the personal space factor and the look radius
  - If you leave the NPCs look radius they will start to follow you
  - The NPC will stop follwing you if you with in the personal space factor - radius.
- You can disable the follow-feature if you disable the NPC Controller script on the NPC (#TODO change the name of the script)

Inspiration/tutorials:

- [Creating your first animated AI Character! [AI #01] by TheKiwiCoder](https://www.youtube.com/watch?v=TpQbqRNCgM0)
- [ENEMY AI - Making an RPG in Unity (E10) by Brackeys](https://www.youtube.com/watch?v=xppompv1DBg&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=11)
  - Youtube video explaining the NavMeshAgent and how to get an enemy to follow the player
