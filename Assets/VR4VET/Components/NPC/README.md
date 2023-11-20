# NPC 👈(⌒▽⌒)👉

## Usage of NPC prefab

- To use the prefab, you need two different files loaded in as Game Object in the scene: 
  - NPCSpawner.cs: Script for spawning a NPC in a scene. Customizable with boolean value for shouldFollow (user) and a position where it should spawn. 
  - EventTrigger.cs: Guidancee cript for triggering spawning and toggling shouldFollow for NPC. This must be loaded with the NPC-prefab located in /NPC folder. This is justan   exmple script which should be used as a skeleton for loading in NPC on specific cases. 

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
 
## Text-To-Speech

- Text-To-Speech works through the Wit.ai SDK, which is designet for Quest XR
  - In the Unity Navbar, under the Oculus tab, there is a tab for the Voice SDK.
  - In here you will find guides for configuration and setup.
- The VR4VET project uses one key for setup to the Wit.ai service, which determines which "configuration" we use.
- To edit the configuration you will need the VR4VET Meta login and change it in Wit.ai

### How to set up TTS

- In the Oculus/Voice SDK tab, go to voice hub and then Configuration. 
  - If needed enter the Wit.ai key to open the configuration.
- When in your scene, right click in the assets area: Create/Voice SDK/TTS/Add Default TTS Setup
- In the scene hierarchy you should now have a TTS object, which contains a TTSWitService object and a TTSSpeaker object.
- The TTSWitService handles communication with the Wit.ai server, and local cache settings
- The TTSSpeaker object is an object that outputs TTS
  - The TTSSpeaker can be dragged out of the TTS object and onto whatever or whichever NPC you want it attached to
  - The TTSSpeaker has a TTSservice reference that should always be connected to the TTSWitService
  - You only need one TTSWitService but can have multiple TTSSpeakers. TO create more use Create/Voice SDK/TTS/Add TTS Speaker to Scene

### Connect the TTS Speaker to an NPC

- Drag a TTSSPeaker object into the NPC object
- The NPC has a script for Dialogue Box Controller. Here all dialogue is sent before being shown to the player
- Click on the NPC object and scroll down to Dialogue Box Controller
  - There is an empty reference to a TTS Speaker
  - Drag and drop the TTSSpeaker you want this NPC to use into this reference

### Modifying the TTS output

- Click on the relevant TTSSpeaker you want to modify
- Choose the Voice Preset that you think fits most, these are premade by Wit.ai
  - The TTSSpeaker object contains a TTSSpeaker Audio object.
  - Here you can chose which priority the TTS audio has, the volume, the panning, spatial blend (2D and 3D audio) and reverb.
    - The 3D sound settings can be used to modify how the audio reacts to the 3D space, volume distance etc...



### Animation and Models from Mixamo
All animation and models are downloaded from mixamo.
#### The animations are downloaded with the following information:

- Y bot / X bot as the character 
  - (The rest of the characters may have buged out rigs, not proporly set to T-pose)
- In place: True
- FBX for Unity (.fbx)
- Without skin
- 30 frames per second
- No keyframe reduction (none)

#### The models have been downloaded with these settings:

- FBX for unity (.fbx)
- T-pose

#### Both animation and characters, the following has been modified in Unity:

- The rig from generic to **humanoid** 
  - (If you run the animation now, you will quicly see if the rig is bugged or not. Look at the feet)
- I have generated the avatar (used later in the animator component)
- I have turned on loop for all the animations
- I have set Root Transform Rotation paramters
  - bake into pose: true
  - based upon: Original

For the models you also need to extract the textures.


### How to add a NPC to a new scene
- Add the necessary prefabs
  - **TTSWitService**
    - Text to speech service
  - **NPCInteractionManager**
    - This is a singleton, and is easly accessed from the other scripts
    - Holds object-reference that the NPC needs both cannot be set in the prefab
    - You **need** to drag and drop the correct values here
- Add/spawn NPC(s) and configure them to your use case
  - The following scripts are found at the root object on the NPC or at the child called *CollisionTriggerHandler*:
    - the script **Coversation controller** holds the NPC dialogue trees
    - the script **Follow The Player Controller** specifies if the NPC should follow after the player and at what distance
    - the script **Set Character Model**
  - You can look at the scripts **NPCSpawner** and **EventTriggerDemo** to see how you would spawn in NPCs





