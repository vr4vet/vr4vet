# NPC 👈(⌒▽⌒)👉

## Dialogue tree
- To use the dialogue tree on an NPC the DialogueBoxController and NPCManager scripts are added to the NPC object as components. 

- A DialogueTree is a scriptable object that can be specified in Unity with dialogue, questions and answer boxed unique to that NPc. 
	- This dialogue tree is then assigned to the NPC in the Dialogue Tree attribute defined in the NPC manager script. 
	- The structs file is used by the DialogueTree. 

- In the DialogueBoxController script, the Dialogue Text, NPC Name Text, Dialogue Box, Answer Box and Answer Objects are assigned.
	- The boxes are containers for the text objects, and the Dialogue text value is sat depending on the dialogue written in the Dialogue Tree object.
	- The Answer Objects is defined by the Answers written in the Dialogue Tree.